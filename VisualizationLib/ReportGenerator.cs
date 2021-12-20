using System;
using Model;
using System.IO;
using System.IO.Compression;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Xml;
using System.Collections.Generic;

namespace VisualizationLib
{
    public static class ReportGenerator
    {
        private static Garment garment;
        private static Review hReview;
        private static Review lReview;
        private static Client hClient;
        private static Client lClient;
        private static List<int> ratings;
        private static double gm_rating;
        public static void Run()
        {
            if(hClient == null)
            {
                hClient = new Client();
                hClient.fullname = "-";
                hClient.birthday_date = DateTime.Now;
                hClient.email = "-";
            }
            if(lClient == null)
            {
                lClient = new Client();
                lClient.fullname = "-";
                lClient.birthday_date = DateTime.Now;
                lClient.email = "-";
            }
            if(hReview == null)
            {
                hReview = new Review();
                hReview.opinion = "";
                hReview.rating = 0;
            }
            if(lReview == null)
            {
                lReview = new Review();
                lReview.opinion = "";
                lReview.rating = 0;
            }
            string zipPath = @"./../data/Sample.docx";
            string extractPath = @"./../data/example";
            ZipFile.ExtractToDirectory(zipPath,extractPath);

            XElement root = XElement.Load(@"./../data/example/word/document.xml");

            FindAndReplace(root);

            root.Save(@"./../data/example/word/document.xml");
            File.Delete(@"./../data/example/word/media/image1.png");
            ChartsGenerator.CreateGarmentRatingsChart(ratings,garment.name,@"./../data/example/word/media/image1.png");

            ZipFile.CreateFromDirectory(@"./../data/example",@"./../data/" +@"/Report "+ DateTime.Now.ToString().Replace("/", ".")+".docx");
            Directory.Delete(@"./../data/example",true);
        }
        public static void SetValues(Garment gm, Review hReview, Review lReview, 
        Client hClient, Client lClient, double rating,List<int> rat)
        {
            garment = gm;
            ReportGenerator.hReview = hReview;
            ReportGenerator.lReview = lReview;
            ReportGenerator.hClient = hClient;
            ReportGenerator.lClient = lClient;
            gm_rating = rating;
            ratings = rat;
        }
        private static void FindAndReplace(XElement node)
        {
            if (node.FirstNode != null
            && node.FirstNode.NodeType == XmlNodeType.Text)
            {
                switch (node.Value)
                {
                    case "{{": node.Value = ""; break;
                    case "}}": node.Value = ""; break;
                    case "name": node.Value = $"{garment.name}"; break;
                    case "brand": node.Value = $"{garment.brand}"; break;
                    case "cost": node.Value = $"{garment.cost}"; break;
                    case "country": node.Value = $"{garment.manufacture_country}"; break;
                    case "rating":
                        if(gm_rating == 0)
                            node.Value = "-";
                        else
                            node.Value = $"{Math.Round(gm_rating,2)}"; 
                        break;
                    case "hOpinion":
                        if(hReview.opinion == "") 
                            node.Value = "-"; 
                        else
                            node.Value = $"{hReview.opinion}";
                        break;
                    case "highestRating": 
                        if(hReview.opinion == "") 
                            node.Value = "-"; 
                        else
                            node.Value = $"{hReview.rating}";
                        break;
                    case "highestPostedAt":
                        if(hReview.opinion == "") 
                            node.Value = "-"; 
                        else
                            node.Value = $"{hReview.posted_at.ToShortDateString()}";
                        break;
                    case "Clienth": 
                        if(hReview.opinion == "") 
                            node.Value = "-"; 
                        else
                        {
                            node.Value = $"{hClient.fullname}";
                        }
                        break;
                    case "lOpinion":
                        if(lReview.opinion == "") 
                            node.Value = "-"; 
                        else
                            node.Value = $"{lReview.opinion}";
                        break;
                    case "lowestRating": 
                        if(lReview.opinion == "") 
                            node.Value = "-"; 
                        else
                            node.Value = $"{lReview.rating}";
                        break;
                    case "lowestPostedAt": 
                        if(lReview.opinion == "") 
                            node.Value = "-"; 
                        else
                            node.Value = $"{lReview.posted_at.ToShortDateString()}";
                        break;
                    case "Clientl" :
                        if(lReview.opinion == "") 
                            node.Value = "-"; 
                        else
                        {
                            node.Value = $"{lClient.fullname}";
                        }
                        break;
                }
            }
            foreach (XElement el in node.Elements())
            {
                FindAndReplace(el);
            }
        } 
    }
}