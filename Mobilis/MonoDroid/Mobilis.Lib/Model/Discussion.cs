using MWC.DL.SQLite;
using System;

namespace Mobilis.Lib.Model
{
    class Discussion
    {
    [PrimaryKey]
	private int _id {get;set;}
    private string name { get; set; }
    private DateTime lastPostDate { get; set; }
    private int status { get; set; }
    private int classId { get; set; }
    private string description { get; set; }
    private int nextPosts { get; set; }
    private int previousPosts { get; set; }
	//private bool hasNewPosts = false {get;set;}
	private DateTime startDate {get;set;}
	private DateTime endDate {get;set;}
    }
}