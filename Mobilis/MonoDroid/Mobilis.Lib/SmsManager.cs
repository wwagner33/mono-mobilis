using Mobilis.Lib.Model;
using System.Collections.Generic;
using System;

namespace Mobilis.Lib
{
    public class SmsManager
    {
        public delegate void FinishedPlaying();
        List<string> blocks;
        private const int MIN_BLOCK_LENGTH = 200;
        private const int MAX_BLOCK_LENGTH = 400;
        private IAsyncPlayer player;

        public SmsManager(IAsyncPlayer player) 
        { 

        }

        public SmsManager() 
        {
           // Para testes
        }

        public void start(Post post) 
        { 
            // separa em blocos e começa as requisições
            createBlocks(post);
            if (blocks.Count > 0) 
            {
                System.Diagnostics.Debug.WriteLine("Printing Blocks");
                foreach (string s in blocks)
                {
                    System.Diagnostics.Debug.WriteLine(s);
                }
            }
        }

        public void createBlocks(Post post) 
        {
            blocks = new List<string>();
            //generateHeader(Post post)
            string content = post.content.Trim();
            int end = content.Length;

            while (end > 0) 
            {
                int cut = Math.Min(content.Length, MIN_BLOCK_LENGTH);
                /*
                if (cut == content.Length)
                {
                    if (containsLetter(content)) // TODO
                    {
                        System.Diagnostics.Debug.WriteLine("BlockContent = " + content);
                        blocks.Add(content);
                    }
                    break;
                }
                 * */
                string blockContent = content.Substring(0, cut);

                content = content.Substring(cut);
                int occurrenceOfDot = content.IndexOf(".") == -1 ? 999 : content
                        .IndexOf(".") + 1;
                int occurrenceOfInterrogation = content.IndexOf("?") == -1 ? 999
                        : content.IndexOf("?") + 1;
                int occurrenceOfExclamation = content.IndexOf("!") == -1 ? 999
                        : content.IndexOf("!") + 1;
                int occurrenceOfSemi = content.IndexOf(";") == -1 ? 999 : content
                        .IndexOf(";") + 1;
                int occurenceOfPause = Math.Min(Math.Min(Math.Min(Math.Min(
                        Math.Min(content.Length, MAX_BLOCK_LENGTH),
                        occurrenceOfDot), occurrenceOfExclamation),
                        occurrenceOfInterrogation), occurrenceOfSemi);
                blockContent = blockContent + content.Substring(0,
                    occurenceOfPause);

                if (occurenceOfPause == content.Length)
                {
                    content = "";
                    end = 0;
                }
                else 
                {
                    content = content.Substring(occurenceOfPause);
                    if (occurenceOfPause == MAX_BLOCK_LENGTH)
                    {
                        blockContent = blockContent + content.Substring(0,
                                content.IndexOf(" "));
                        content = content.Substring(content.IndexOf(" "));
                    }
                    end = content.Length;
                }
                System.Diagnostics.Debug.WriteLine("Block content = " + blockContent);

                if (blockContent != null)
                {
                    //blocks.addBlock(blockContent);
                    blocks.Add(blockContent);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("ContentBlock is null");
                    break;
                }
                System.Diagnostics.Debug.WriteLine("BlockLenght = " + blocks.Count);
            }
        }

        /*
        public bool containsLetter(string content) 
        {
       
        }
         * */

        public void generateHeader(Post post) 
        {
            /*
            string header = post.userName();
            String date = posts.get(postIndex).getDate();
            int year = Integer.parseInt(date.substring(0, 4));
            int month = Integer.parseInt(date.substring(5, 7));
            int day = Integer.parseInt(date.substring(8, 10));
            int hour = Integer.parseInt(date.substring(11, 13));
            int minute = Integer.parseInt(date.substring(14, 16));
             * */
        }
    }
}