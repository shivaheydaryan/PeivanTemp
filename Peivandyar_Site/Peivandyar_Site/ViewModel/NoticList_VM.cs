using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewModel
{
    public class NoticList_VM
    {
        public int id { get; set; }
        public string username_sender { get; set; }
        public string Title { get; set; }
        public string Msg_Content { get; set; }
        public DateTime? Date { get; set; }
        public byte? Type { get; set; }
        public int instituteid { get; set; }
        public byte? status_Receiver { get; set; }
        public bool? Attach { get; set; }
        public string sender { get; set; }
        public string name { get; set; }

    }
}