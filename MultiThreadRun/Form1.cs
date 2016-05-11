using System;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Collections.Concurrent;

namespace MultiThreadRun
{
    public partial class Form1 : Form
    {
        const ulong ensuranceMem = 314572800;   // The amount of available memory to leave out

        ConcurrentQueue<string> cmdQueue;
        Process[] processes;
        string workingDir;
        int endCheckCount = 0;

        public Form1()
        {
            InitializeComponent();

            numThreadBox.Maximum = Environment.ProcessorCount - 1;

            timer.Tick += delegate
            {
                lock (processes)
                    for (int i = 0; i < processes.Length; ++i)
                        if (!processes[i].HasExited) return;

                ++endCheckCount;
                if (endCheckCount == 3)
                {
                    if (!cmdQueue.IsEmpty) statusTextBox.AppendText("Error : can't start anymore processes!");
                    toggleControls();
                    timer.Enabled = false;
                }
            };
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            lock (runButton)
                if (!runButton.Visible) stopButton_Click(null, null);
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            toggleControls();
            statusTextBox.Text = "";

            loadCmdQueue(cmdPathTextBox.Text);
            processes  = new Process[(int)numThreadBox.Value];
            workingDir = Path.GetDirectoryName(cmdPathTextBox.Text);
            
            for (int i = 0; i < processes.Length; ++i)
            {
                processes[i] = new Process();
                processes[i].EnableRaisingEvents = true;
                processes[i].Exited += doMoreCmd;
            }
            doMoreCmd(null, null);

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
            timer.Enabled = false;

            lock (processes)
            {
                for (int i = 0; i < processes.Length; ++i)
                    if (!processes[i].HasExited)
                        try
                        {
                            processes[i].Exited -= doMoreCmd;
                            processes[i].Kill();
                            processes[i].Refresh();
                        } catch (Exception) { }
            }

            toggleControls();
            cmdQueue = null;
        }

        private void useMemoryLimiterCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            memoryLimiterBrowseButton.Enabled = memoryLimiterPathBox.Enabled ^= true;
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

            var fi = new StreamReader(path);
            while (!fi.EndOfStream)
            {
                string cmd = fi.ReadLine();
                if (!string.IsNullOrEmpty(cmd)) cmdQueue.Enqueue(cmd);
            }

            fi.Close();
        }

        private bool enoughMemoryForCmd (string cmd)
        {
            Process getMemoryProcess = new Process();
            getMemoryProcess.StartInfo             = getProcessInfo(cmd);
            getMemoryProcess.StartInfo.FileName    = memoryLimiterPathBox.Text;
            getMemoryProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            getMemoryProcess.Start();
            getMemoryProcess.WaitForExit();

            if (getMemoryProcess.ExitCode != 0)
            {
                statusTextBox.AppendText("Memory limiter exited with error for : " + cmd + "\n");
                return false;
            }

            ulong requiredMem  = ulong.Parse(getMemoryProcess.StandardOutput.ReadLine());
            ulong availableMem = (new Microsoft.VisualBasic.Devices.ComputerInfo()).AvailablePhysicalMemory;

            return requiredMem + ensuranceMem < availableMem;
        }

        private void doMoreCmd (object o,EventArgs e)
        {
            (o as Process)?.Refresh();

            lock (processes)
                while (true)
                {
                    string cmd;
                    if (!cmdQueue.TryPeek(out cmd)) break;
                    if (useMemoryLimiterCheckBox.Checked && !enoughMemoryForCmd(cmd)) break;

                    bool stopFlag = true;
                    for (int i = 0; i < processes.Length; ++i)
                    {
                        try
                        {
                            if (!processes[i].HasExited) continue;
                        }
                        catch (InvalidOperationException) { }

                        lock (statusTextBox) statusTextBox.AppendText(cmd + "\n");
                        processes[i].StartInfo = getProcessInfo(cmd);
                        processes[i].Start();

                        stopFlag = false;
                        cmdQueue.TryDequeue(out cmd);
                        break;
                    }

                    if (stopFlag) break;
                }
        }

        private void toggleControls ()
        {
            if (runButton.Visible)
            {
                cmdPathTextBox.Enabled =
                numThreadBox.Enabled =
                useMemoryLimiterCheckBox.Enabled =
                memoryLimiterPathBox.Enabled =
                memoryLimiterBrowseButton.Enabled =
                runButton.Visible =
                false;
            } else
            {
                cmdPathTextBox.Enabled =
                numThreadBox.Enabled =
                useMemoryLimiterCheckBox.Enabled =
                runButton.Visible =
                true;

                memoryLimiterPathBox.Enabled = memoryLimiterBrowseButton.Enabled = useMemoryLimiterCheckBox.Checked;
            }
        }
    }
}
