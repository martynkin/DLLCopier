using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DllCopier
{

master6
master6
master6
master6master6

    public enum FileActionsType : int
    {
        faCopy = 1,
        faDelete = 2,
        faRename = 3,
        faNone = 4
    };

    public class FileActionEventArgs : EventArgs
    {
        private string fa_sMessage;
        private FileActionsType fa_Type;
        private object fa_oActionData;

        public FileActionEventArgs()
        {
            fa_sMessage = "";
            fa_Type = FileActionsType.faNone;
            fa_oActionData = null;
        }

        public FileActionEventArgs(string msg, FileActionsType faType, object oData)
        {
            fa_sMessage = msg;
            fa_Type = faType;
            fa_oActionData = oData;
        }

        public string Message
        {
            set
            {
                fa_sMessage = value;
            }
            get
            {
                return fa_sMessage;
            }
        }

        public FileActionsType ActionType
        {
            set
            {
                fa_Type = value;
            }
            get
            {
                return fa_Type;
            }
        }

        public object ActionData
        {
            set
            {
                fa_oActionData = value;
            }
            get
            {
                return fa_oActionData;
            }
        }
    }


    class FileCopier
    {
        private const int iCopyBufferLenght = 2048;

        public delegate void CopyCompleteEventHandler(object sender, FileActionEventArgs e);
        public delegate void CopyProgressEventHandler(object sender, FileActionEventArgs e);

        public event CopyCompleteEventHandler OnCopyComplete;
        public event CopyProgressEventHandler OnCopyProgress;


        public void Copy(object oParametersList)
        {
            FileActionEventArgs e = new FileActionEventArgs();
            e.ActionType = FileActionsType.faCopy;
            e.Message = "Copy starts!";
            e.ActionData = null;

            int numReads = 0;
            long totalBytesRead = 0;
            Byte[] bCopyBuffer = new Byte[iCopyBufferLenght];

            string[] lParams = new string[2];
            lParams = (string[])oParametersList;

            FileStream fsSourceStream = new FileStream(lParams[0], FileMode.Open, FileAccess.Read);
            FileStream fsDestStream = new FileStream(lParams[1], FileMode.Create, FileAccess.Write);

            long sLenght = fsSourceStream.Length;

            try
            {
                while (true)
                {
                    int bytesRead = 0;
                    numReads++;
                    bytesRead = fsSourceStream.Read(bCopyBuffer, 0, iCopyBufferLenght);

                    if (bytesRead == 0)
                    {
                        SendInfo(sLenght, sLenght);
                        break;
                    }


                    fsDestStream.Write(bCopyBuffer, 0, bytesRead);
                    totalBytesRead += bytesRead;


                    if (numReads % 10 == 0)
                    {
                        int iprocNext = SendInfo(totalBytesRead, sLenght);
                    }


                    if (bytesRead < iCopyBufferLenght)
                    {
                        int iprocNext = SendInfo(totalBytesRead, sLenght);
                        break;
                    }
                }

                fsSourceStream.Close();
                fsDestStream.Close();

                if (OnCopyComplete != null)
                    OnCopyComplete(this, new FileActionEventArgs("Copy complete", FileActionsType.faCopy, null));
            }
            catch
            {
                if (OnCopyComplete != null)
                    OnCopyComplete(this, new FileActionEventArgs("Copy complete", FileActionsType.faCopy, null));
            }

        }

        private int SendInfo(long totalBytesRead, long sLenght)
        {
            double pctDone = (double)((double)totalBytesRead / (double)sLenght);

            if (OnCopyProgress != null && !double.IsNaN(pctDone))
            {
                List<long> llCopyProgressData = new List<long>();
                llCopyProgressData.Add(sLenght);
                llCopyProgressData.Add(totalBytesRead);
                llCopyProgressData.Add((long)(pctDone * 100));

                OnCopyProgress(this, new FileActionEventArgs("Copy in progress", FileActionsType.faCopy, llCopyProgressData));
            }
            return (int)(pctDone * 100);
        }


        public void GetFileInfo()
        { 
        }
    }
}
