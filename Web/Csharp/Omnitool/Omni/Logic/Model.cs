using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Microsoft.Net.Http.Headers;
using Omnitool.Models;

public class Model
{
    public List<Chat> chats{get; set;}
    public List<Currency> currencies{get; set;}
    public List<Crypto> cryptos{get; set;}
    public List<Finance> finances{get; set;}
    
    public string[] translate {get; set;}
}
