using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThuPhi.Droid.Renderers;
using ThuPhi.Model.Send;
using ThuPhi.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(SmsAndroid))]
namespace ThuPhi.Droid.Renderers
{
    public class SmsAndroid : ISms
    {
        Context context = Android.App.Application.Context;
        string INBOX = "content://sms/inbox";
        string[] reqCols = new string[] { "_id", "thread_id", "address", "person", "date", "body", "type" };

        public List<User> GetAll()
        {
            var users = new List<User>();
            Android.Net.Uri uri = Android.Net.Uri.Parse(INBOX);
            var cursor = context.ContentResolver.Query(uri, reqCols, null, null, null);
            if (cursor.MoveToFirst())
            {
                do
                {
                    //string messageId = cursor.GetString(cursor.GetColumnIndex(reqCols[0]));
                    //string threadId = cursor.GetString(cursor.GetColumnIndex(reqCols[1]));
                    string address = cursor.GetString(cursor.GetColumnIndex(reqCols[2]));
                    //string name = cursor.GetString(cursor.GetColumnIndex(reqCols[3]));
                    string date = cursor.GetString(cursor.GetColumnIndex(reqCols[4]));
                    string msg = cursor.GetString(cursor.GetColumnIndex(reqCols[5]));
                    //string type = cursor.GetString(cursor.GetColumnIndex(reqCols[6]));

                    users.Add(new User
                    {
                        Address = address,
                        Content = msg,
                        LongTime = date,
                    });
                } while (cursor.MoveToNext());
            }

            return users;
        }

        string fliterDateTime(DateTime start)
        {
            string result = null;

            if (start != DateTime.MinValue || start != DateTime.MaxValue)
            {
                return "date >= " + (start.Ticks / 10000000 - 62135596800);
            }

            return result;
        }

        string fliterAddress(string[] addresss)
        {
            string result = "";

            foreach (var item in addresss)
            {
                result += "address" + $" = '{item}'" + " or ";
            }

            return result.Substring(0, result.Length - 4); 
        }

        string fliterContent(string para)
        {
            string result = "";

            result = "body" + $" LIKE '%{para}%'";

            return result;
        }

        string fliterContentAndDateTime(string para, DateTime time)
        {
            return fliterContent(para) + " and " + fliterDateTime(time);
        }

        public List<User> GetByDateTime(DateTime start)
        {
            var users = new List<User>();
            Android.Net.Uri uri = Android.Net.Uri.Parse(INBOX);
            var cursor = context.ContentResolver.Query(uri, reqCols, fliterDateTime(start), null, null);
            if (cursor.MoveToFirst())
            {
                do
                {
                    string address = cursor.GetString(cursor.GetColumnIndex(reqCols[2]));
                    string date = cursor.GetString(cursor.GetColumnIndex(reqCols[4]));
                    string msg = cursor.GetString(cursor.GetColumnIndex(reqCols[5]));

                    users.Add(new User
                    {
                        Address = address,
                        Content = msg,
                        LongTime = date,
                    });
                } while (cursor.MoveToNext());
            }

            return users;
        }

        public List<User> GetByAddress(string[] addresss)
        {
            var users = new List<User>();
            Android.Net.Uri uri = Android.Net.Uri.Parse(INBOX);
            var cursor = context.ContentResolver.Query(uri, reqCols, fliterAddress(addresss), null, null);
            if (cursor.MoveToFirst())
            {
                do
                {
                    string address = cursor.GetString(cursor.GetColumnIndex(reqCols[2]));
                    string date = cursor.GetString(cursor.GetColumnIndex(reqCols[4]));
                    string msg = cursor.GetString(cursor.GetColumnIndex(reqCols[5]));

                    users.Add(new User
                    {
                        Address = address,
                        Content = msg,
                        LongTime = date,
                    });
                } while (cursor.MoveToNext());
            }

            return users;
        }

        public List<User> GetByContent(string para)
        {
            var users = new List<User>();
            Android.Net.Uri uri = Android.Net.Uri.Parse(INBOX);
            var cursor = context.ContentResolver.Query(uri, reqCols, fliterContent(para), null, null);
            if (cursor.MoveToFirst())
            {
                do
                {
                    string address = cursor.GetString(cursor.GetColumnIndex(reqCols[2]));
                    string date = cursor.GetString(cursor.GetColumnIndex(reqCols[4]));
                    string msg = cursor.GetString(cursor.GetColumnIndex(reqCols[5]));

                    users.Add(new User
                    {
                        Address = address,
                        Content = msg,
                        LongTime = date,
                    });
                } while (cursor.MoveToNext());
            }

            return users;
        }

        public List<User> GetByContentAndDateTime(string para, DateTime start)
        {
            var users = new List<User>();
            Android.Net.Uri uri = Android.Net.Uri.Parse(INBOX);
            var cursor = context.ContentResolver.Query(uri, reqCols, fliterContentAndDateTime(para, start), null, null);
            if (cursor.MoveToFirst())
            {
                do
                {
                    string address = cursor.GetString(cursor.GetColumnIndex(reqCols[2]));
                    string date = cursor.GetString(cursor.GetColumnIndex(reqCols[4]));
                    string msg = cursor.GetString(cursor.GetColumnIndex(reqCols[5]));

                    users.Add(new User
                    {
                        Address = address,
                        Content = msg,
                        LongTime = date,
                    });
                } while (cursor.MoveToNext());
            }

            return users;
        }
    }
}