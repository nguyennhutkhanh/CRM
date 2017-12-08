using System;

namespace WcfService.Format
{
    class Stream_JSON
    {
        static public string StreamToJSON(string s)
        {
            //TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();
            //return await tcs.Task;
            //http://stackoverflow.com/questions/33461827/how-can-i-use-a-method-async-taskstring-and-return-string-and-then-how-do-i-p

            return s.Replace(Environment.NewLine, "")
                                                       .Replace("\\r", "")
                                                       .Replace("\\n", "")
                                                       .Replace("&#xD;&#xA;", "")
                                                       .Replace("&#xA;", "")
                                                       .Replace("&#xD;", "")
                                                       .Replace("\u000a\u000d", "")
                                                       .Replace("\u000a", "")
                                                       .Replace("\u000d;", "");

        }
    }
}
