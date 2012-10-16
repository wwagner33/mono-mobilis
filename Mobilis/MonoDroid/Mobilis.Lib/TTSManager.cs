using Mobilis.Lib.Model;
using System.Collections.Generic;
using System;
using Mobilis.Lib.DataServices;
using System.Threading;
using Mobilis.Lib.Util;
using System.IO;

namespace Mobilis.Lib
{
    public class TTSManager
    {
        public delegate void BlockFinishedPlaying();
        public delegate void PostFinishedPlaying();
        private List<string> blocks;
        private const int MIN_BLOCK_LENGTH = 200;
        private const int MAX_BLOCK_LENGTH = 400;
        private IAsyncPlayer player;
        private BingService bingService;
        bool[] blocksAvaliability;
        int playedLast = -1;
        bool isPlaying = false;

        public TTSManager(IAsyncPlayer player) 
        {
            this.player = player;
            bingService = new BingService();
        }

        public TTSManager() 
        {
           //Para testes
           bingService = new BingService();
        }
        
        public void start(Post post) 
        { 
            // separa em blocos e começa as requisições
            // Para TODAS as threads secundárias criadas pela aplicação.
            createBlocks(post);
            makeRequests(post);
        }

        private void playAudioBlock(int blockId) 
        {
            System.Diagnostics.Debug.WriteLine("Tocando Bloco " + blockId);
            isPlaying = true;
            playedLast = blockId;
            player.play(blockId, () => 
            {
                if (blockId == (blocks.Count - 1)) 
                {
                    // última posição.
                    System.Diagnostics.Debug.WriteLine("Última posição tocada, parando áudio.");
                    isPlaying = false;
                }
                else if (blocksAvaliability[(blockId + 1)])
                {
                    // Toca o próximo bloco
                    System.Diagnostics.Debug.WriteLine("Bloco " + blockId + " Tocado, Tocando bloco " + (blockId+1));
                    playAudioBlock((blockId + 1));
                }
                else 
                {
                    // Bloco não baixado ainda
                    System.Diagnostics.Debug.WriteLine("Próximo bloco ainda não foi baixado. Aguardando.");
                    isPlaying = false;
                }
            });
          }
        
        private void makeRequests(Post post) // TODO uma Thread para cada requisição
        {
           ThreadPool.QueueUserWorkItem(state => 
           {
                for (int i = 0; i < blocks.Count; i++)
               {
                    bingService.GetAsAudio2(blocks[i], i, r =>
                    {
                       System.Diagnostics.Debug.WriteLine("Block " + r + " Downloaded");
                        blocksAvaliability[r] = true;
                        if (r == 0) 
                        {
                            playAudioBlock(r); 
                        }
                        else if ((r - 1 == playedLast) && !isPlaying) 
                        {
                            playAudioBlock(r);
                        }
                    });
                }
            });
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
                    blocks.Add(blockContent);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("ContentBlock is null");
                    break;
                }
            }
            System.Diagnostics.Debug.WriteLine("BlockLenght = " + blocks.Count);
            blocksAvaliability = new bool[blocks.Count];
        }

        /*
        public bool containsLetter(string content) 
        {
        }
        */

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
        public void releaseResources() 
        {
            /*Liberação de recursos quando ocorrer stop,next ou previous*/
            bingService.abortTTSRequests();
            player.reset();
            deleteTTSFiles();
        }
        public void deleteTTSFiles()
        {
            System.IO.DirectoryInfo downloadedMessageInfo = new DirectoryInfo(Constants.RECORGING_PATH);

            foreach (FileInfo file in downloadedMessageInfo.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in downloadedMessageInfo.GetDirectories())
            {
                dir.Delete(true);
            }
        }
    }
}