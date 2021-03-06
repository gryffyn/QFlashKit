﻿using System;
using System.Diagnostics;
using System.Text;
using QFlashKit.code.data;
using QFlashKit.code.module;

namespace QFlashKit.code.Utility
{
    public class Cmd
    {
        private string devicename;

        public Cmd(string _deivcename)
        {
            devicename = _deivcename;
        }

        public string Execute(string deviceName, string dosCommand)
        {
            return Execute(deviceName, dosCommand, 0);
        }

        public string Execute_returnLine(string deviceName, string dosCommand)
        {
            return Execute_returnLine(deviceName, dosCommand, 1);
        }

        public string Execute(string deviceName, string command, int seconds)
        {
            var str = "";
            if (command != null && !command.Equals(""))
            {
                var process = initCmdProcess(command);
                try
                {
                    if (process.Start())
                    {
                        if (seconds == 0)
                            process.WaitForExit();
                        else
                            process.WaitForExit(seconds);
                        str = process.StandardOutput.ReadToEnd();
                    }
                }
                catch
                {
                }
                finally
                {
                    if (process != null)
                        process.Close();
                }
            }

            return str;
        }

        public string Execute_returnLine(string deviceName, string command, int seconds)
        {
            devicename = deviceName;
            var stringBuilder = new StringBuilder();
            if (command != null && !command.Equals(""))
            {
                var process = initCmdProcess(command);
                process.EnableRaisingEvents = true;
                try
                {
                    process.OutputDataReceived += process_OutputDataReceived;
                    process.ErrorDataReceived += process_ErrorDataReceived;
                    process.EnableRaisingEvents = true;
                    process.Exited += process_Exited;
                    if (process.Start())
                    {
                        process.BeginOutputReadLine();
                        process.BeginErrorReadLine();
                        process.WaitForExit();
                    }
                }
                catch (Exception ex)
                {
                    Log.w(deviceName, ex, true);
                }
                finally
                {
                    if (process != null)
                        process.Close();
                }
            }

            return stringBuilder.ToString();
        }

        private void process_Exited(object sender, EventArgs e)
        {
            Log.w(devicename, "flash done");
            FlashingDevice.UpdateDeviceStatus(devicename, 1f, "flash done", "success", true);
        }

        private void process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Data))
                return;
            Log.w(devicename, e.Data);
        }

        private void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Data))
                return;
            if (e.Data.IndexOf("error") >= 0 || e.Data.IndexOf("fail") >= 0)
                throw new Exception(e.Data);
            var data = e.Data;
            Log.w(devicename, e.Data);
            if (data.IndexOf("flash") < 0)
                return;
            FlashingDevice.UpdateDeviceStatus(devicename, GetPercent(data), data, "flashing", false);
        }

        private Process initCmdProcess(string command)
        {
            return new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/C " + command,
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };
        }

        private float? GetPercent(string line)
        {
            var dummyProgress = SoftwareImage.DummyProgress;
            var nullable = new float?();
            foreach (string key in dummyProgress.Keys)
                if (line.IndexOf(key) >= 0)
                {
                    nullable = Convert.ToInt32(dummyProgress[key]) / 50f;
                    break;
                }

            return nullable;
        }
    }
}