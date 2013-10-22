using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace FeatureDealer.RuLemmatizer
{
    internal class Lemmatizer : IDisposable
    {
        private const string mystemFileName = "mystem.exe";
        private readonly Process myStemProcess;
        private List<string> processOutputLines = new List<string>();
        private bool processGivesOutput = true;

        public Lemmatizer()
        {
            myStemProcess = GetMyStemProcess();
            myStemProcess.Start();
            myStemProcess.BeginOutputReadLine();
        }

        #region IDisposable Members

        public void Dispose()
        {
            myStemProcess.Close();
        }

        #endregion

        private Process GetMyStemProcess()
        {
            var process = new Process
                              {
                                  StartInfo =
                                      {
                                          FileName = mystemFileName,
                                          Arguments = "-in -e utf-8",
                                          UseShellExecute = false,
                                          CreateNoWindow = true,
                                          RedirectStandardOutput = true,
                                          StandardOutputEncoding = Encoding.UTF8,
                                          RedirectStandardInput = true,
                                          RedirectStandardError = true
                                      }
                              };
            process.OutputDataReceived += ProcessOutputString;
            return process;
        }

        public IEnumerable<LemmaInfo> GetLemmaInfos(params string[] texts)
        {
            processOutputLines = new List<string>();
            processGivesOutput = true;
            byte[] bytes = Encoding.UTF8.GetBytes(string.Join(" ", texts) + " INPUTENDMARKER\r\n");
            myStemProcess.StandardInput.BaseStream.Write(bytes, 0, bytes.Length);
            myStemProcess.StandardInput.Flush();
            while (processGivesOutput)
            {
            }
            IEnumerable<LemmaInfo> lemmaInfos = processOutputLines.Select(ParseOutputLine);
            return lemmaInfos;
        }

        private static LemmaInfo ParseOutputLine(string line)
        {
            string[] strArray = line.Split(new char[5]
                                               {
                                                   '{',
                                                   '=',
                                                   ',',
                                                   '?',
                                                   '}'
                                               }, StringSplitOptions.RemoveEmptyEntries);
            if (strArray.Length <= 2)
                return new LemmaInfo(strArray[0], "unknown", "unknown");
            return new LemmaInfo(strArray[0], strArray[1], strArray[2]);
        }

        private void ProcessOutputString(object sender, DataReceivedEventArgs outLine)
        {
            if (outLine.Data == null || outLine.Data.StartsWith("INPUTENDMARKER"))
                processGivesOutput = false;
            else
                processOutputLines.Add(outLine.Data);
        }
    }
}