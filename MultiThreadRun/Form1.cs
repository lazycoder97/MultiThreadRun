using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace MultiThreadRun
{
    public partial class Form1 : Form
    {
        private const ulong ensuranceMemory = 314572800;                // The amount of available memory to leave out

        private ConcurrentDictionary<Process, DataGridViewRow> cmdRow;  // statusTable's row corresponding to process
        private ConcurrentQueue<string> cmdQueue;                       // The command queue
        private Process[] processes;                                    // List of processes to execute commands
        private string workingDir;                                      // Path to the working directory
        private int endCheckCount;                                      // ---

        public Form1()
        {
            InitializeComponent();

            // Set the number of computer thread as the maximum allowed paralel run
            numThreadBox.Maximum = Environment.ProcessorCount - 1;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Stop all processes before closing
            if (!runButton.Visible) stopButton_Click(null, null);
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            // Disable controls and reset statusTable
            toggleControls();
            runOnUIThread(() => statusTable.Rows.Clear());

            // Initialize
            loadCmdQueue(cmdPathTextBox.Text);
            processes  = new Process[(int)numThreadBox.Value];
            cmdRow = new ConcurrentDictionary<Process, DataGridViewRow>();
            workingDir = Path.GetDirectoryName(cmdPathTextBox.Text);

            for (int i = 0; i < processes.Length; ++i)
            {
                processes[i] = new Process();
                processes[i].EnableRaisingEvents = true;
                processes[i].Exited += doMoreCmd;
            }

            // Start processes
            doMoreCmd(null, null);

            // Start timer
            endCheckCount = 0;
            timer.Enabled = true;
        }

        private void cmdFileBrowseButton_Click(object sender, EventArgs e)
        {
            if (fileOpenner.ShowDialog() == DialogResult.OK)
                cmdPathTextBox.Text = fileOpenner.FileName;
        }

        private void memoryLimiterBrowseButton_Click(object sender, EventArgs e)
        {
            if (fileOpenner.ShowDialog() == DialogResult.OK)
                memoryLimiterPathBox.Text = fileOpenner.FileName;
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            // Disable timer
            timer.Enabled = false;

            // Kill all processes still running
            lock (processes)
            {
                for (int i = 0; i < processes.Length; ++i)
                    if (isRunning(processes[i]))
                        try
                        {
                            processes[i].Exited -= doMoreCmd;
                            processes[i].Kill();
                            processes[i].Dispose();

                            DataGridViewRow row;
                            cmdRow.TryRemove(processes[i], out row);
                            runOnUIThread(() => row.DefaultCellStyle.BackColor = Color.OrangeRed);
                        }
                        catch (Exception) { }
            }

            // Restore initial state of form
            toggleControls();
            cmdQueue = null;
            cmdRow = null;
            processes = null;
        }

        private void useMemoryLimiterCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            memoryLimiterBrowseButton.Enabled = memoryLimiterPathBox.Enabled ^= true;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            // Check if there is still a running process
            lock (processes)
                for (int i = 0; i < processes.Length; ++i)
                    if (isRunning(processes[i])) { endCheckCount = 0; return; }

            ++endCheckCount;
            if (endCheckCount == 3)
            {
                // If there is still commands left
                if (!cmdQueue.IsEmpty) MessageBox.Show("Error : can't start anymore processes!");

                // Restore initial state of form
                toggleControls();
                cmdQueue = null;
                cmdRow = null;
                processes = null;
                timer.Enabled = false;
            }
        }

        private void statusTable_SelectionChanged(object sender, EventArgs e)
        {
            statusTable.ClearSelection();
        }

        private ProcessStartInfo getProcessInfo(string cmd)
        {
            string name = cmd,
                   args = "";
            int splitIndex = cmd.IndexOf(' ');

            if (splitIndex != -1)
            {
                name = cmd.Substring(0, splitIndex);
                args = cmd.Substring(splitIndex + 1);
            }

            var info = new ProcessStartInfo();
            info.FileName = name;
            info.Arguments = args;
            info.WindowStyle = ProcessWindowStyle.Minimized;
            info.WorkingDirectory = workingDir;

            return info;
        }

        private void loadCmdQueue(string path)
        {
            cmdQueue = new ConcurrentQueue<string>();

            try
            {
                var fi = new StreamReader(path);
                while (!fi.EndOfStream)
                {
                    string cmd = fi.ReadLine();
                    if (!string.IsNullOrEmpty(cmd)) cmdQueue.Enqueue(cmd);
                }

                fi.Close();
            } catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private ulong getRequiredMemory(string cmd)
        {
            // Set memory estimator
            Process getMemoryProcess = new Process();
            getMemoryProcess.StartInfo = getProcessInfo(cmd);
            getMemoryProcess.StartInfo.FileName = memoryLimiterPathBox.Text;
            getMemoryProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            // Run memory estimator
            try
            {
                getMemoryProcess.Start();
                getMemoryProcess.WaitForExit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return ulong.MaxValue;
            }

            // Check for errors and return
            if (getMemoryProcess.ExitCode != 0) return ulong.MaxValue;

            try { return ulong.Parse(getMemoryProcess.StandardOutput.ReadLine()); }
            catch (Exception) { return ulong.MaxValue; }
        }

        private void doMoreCmd(object o, EventArgs e)
        {
            // Mark as done in statusTable
            if (o != null)
            {
                (o as Process).Refresh();

                DataGridViewRow row;
                cmdRow.TryRemove(o as Process, out row);
                runOnUIThread(() => row.DefaultCellStyle.BackColor = Color.LimeGreen);
            }

            // Get current available memory
            ulong freeMemory = (new Microsoft.VisualBasic.Devices.ComputerInfo()).AvailablePhysicalMemory;

            // Launch more commands
            lock (processes)
            {
                for (int i = 0; i < processes.Length; ++i)
                {
                    // Check if thread is already running
                    if (isRunning(processes[i])) continue;

                    // Get new command
                    string cmd;
                    if (!cmdQueue.TryPeek(out cmd)) break;
                    
                    if (useMemoryLimiterCheckBox.Checked)
                    {
                        ulong requiredMemory = getRequiredMemory(cmd);
                        if (requiredMemory + ensuranceMemory > freeMemory) break;
                        freeMemory -= requiredMemory;
                    }

                    // Add new row to statusTable
                    runOnUIThread(() =>
                    {
                        var newRow = statusTable.Rows[statusTable.Rows.Add()];
                        newRow.Cells[0].Value = cmd;
                        newRow.DefaultCellStyle.BackColor = Color.Gold;
                        cmdRow[processes[i]] = newRow;
                    });

                    // Launch command
                    processes[i].StartInfo = getProcessInfo(cmd);
                    processes[i].Start();

                    // Get command out of queue
                    cmdQueue.TryDequeue(out cmd);
                    cmd = null;
                }
            }
        }

        private void toggleControls()
        {
            runOnUIThread(() =>
            {
                if (runButton.Visible)
                {
                    cmdPathTextBox.Enabled =
                    cmdFileBrowseButton.Enabled =
                    numThreadBox.Enabled =
                    useMemoryLimiterCheckBox.Enabled =
                    memoryLimiterPathBox.Enabled =
                    memoryLimiterBrowseButton.Enabled =
                    runButton.Visible =
                    false;
                }
                else
                {
                    cmdPathTextBox.Enabled =
                    cmdFileBrowseButton.Enabled =
                    numThreadBox.Enabled =
                    useMemoryLimiterCheckBox.Enabled =
                    runButton.Visible =
                    true;

                    memoryLimiterPathBox.Enabled = memoryLimiterBrowseButton.Enabled = useMemoryLimiterCheckBox.Checked;
                }
            });
        }

        private void runOnUIThread(Action act)
        {
            this.Invoke(act);
        }

        private bool isRunning(Process process)
        {
            try { Process.GetProcessById(process.Id); }
            catch (Exception) { return false; }
            return true;
        }
    }
}