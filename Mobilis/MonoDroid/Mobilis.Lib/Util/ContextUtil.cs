using System;
using System.Collections.Generic;
using System.Text;

namespace Mobilis.Lib.Util
{
    public class ContextUtil
    {
        private static ContextUtil instance;
        public int Course {get;set;}
        public int Class {get;set;}
        public int Discussion {get;set;}
        public int Post {get; set;}
        public int postsAfter {get; set;}
        public int postsBefore {get; set;}

        private ContextUtil() { }

        public static ContextUtil Instance 
        {
            get 
            {
                if (instance == null) 
                {
                    instance = new ContextUtil();
                }
                return instance;
            }
        }
    }
}
