using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] arIDs = { "bt147", "bt148", "bt149", "bt150", "bt153" };
            Counter  = ConsoleApplication1.Properties.Settings.Default.Count;
            
            for(int i = 0; i <arIDs.Length; i ++)
            {
                 ThreadPara p = new ThreadPara() {  sID = arIDs[i], ThreadNo = i};
                System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(DoWork), p);
                Console.WriteLine("Start Thread " + p.sID);
            }
            Console.WriteLine("按回车键关闭...");
            Console.ReadLine();
            ConsoleApplication1.Properties.Settings.Default.Count = Counter;
            ConsoleApplication1.Properties.Settings.Default.Save();
        }

        static int Counter = 0;
        public static void IncCounter()
        {
            Counter++; 
            Console.SetCursorPosition(1, 8);
            Console.Write("Counter: " + Counter + "       ");
            
        }

        class ThreadPara
        {
            public string sID;
            public int ThreadNo;
        }

        private static void DoWork(object para)
        {
            ThreadPara p = para as ThreadPara;
            string sID = p.sID;
            int iThreadNo = p.ThreadNo;

            while (true)
            {
                lock (typeof(Program))
                {
                    
                    using (WebClient wc = new WebClient())
                    {
                        wc.Headers.Add("Accept", "*/*");
                        wc.Headers.Add("Accept-Encoding", "gzip,deflate,sdch");
                        wc.Headers.Add("Accept-Language", "zh-CN,zh;q=0.8");
                        wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                        wc.Headers.Add("Host", "toupiao.sinoins.com:8080");
                        wc.Headers.Add("Origin", "http://toupiao.sinoins.com:8080");
                        wc.Headers.Add("Referer", "http://toupiao.sinoins.com:8080/Vote/index.jsp");
                        wc.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.57 Safari/537.36");
                        wc.Headers.Add("Cookie", "JSESSIONID=491537CC671FFE57C880F69689744924; Hm_lvt_81f3ff41067778d689b7b5dd22bf6d15=1386122062; Hm_lpvt_81f3ff41067778d689b7b5dd22bf6d15=1386122725; AJSTAT_ok_pages=3; AJSTAT_ok_times=1");
                        wc.Headers.Add("X-Requested-With", "XMLHttpRequest");
                        try
                        {
                            string result = wc.UploadString("http://toupiao.sinoins.com:8080/Vote/voteUser.do?p=addCandidateVotes", "candidateId=" + sID);
                            Console.SetCursorPosition(1, iThreadNo + 10); 
                            Console.Write(sID + ":" + result + "           ");
                            IncCounter();
                        }
                        catch (System.Threading.ThreadAbortException ex)
                        {
                            Console.SetCursorPosition(1, iThreadNo + 10);
                            Console.Write(sID + ":" + ex.Message + "           ");
                            return;
                        }
                        catch (System.Threading.ThreadInterruptedException ex)
                        {
                            Console.SetCursorPosition(1, iThreadNo + 10);
                            Console.Write(sID + ":" + ex.Message + "           ");
                            return;
                        }
                        catch (Exception ex)
                        {
                            Console.SetCursorPosition(1, iThreadNo + 10);
                            Console.Write(sID + ":" + ex.Message + "           ");
                            continue;
                        }
                    }
                }
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}
