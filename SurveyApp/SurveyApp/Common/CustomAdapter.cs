﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Service.Notification;
using Android.Views;
using Android.Widget;
using Java.Lang;
using SurveyApp.Model;

namespace SurveyApp
{
    class CustomAdapter : BaseAdapter
    {
        private Context context;
        private List<Ranking> lstRanking;
        private Score score;
       

        public CustomAdapter(Context context, List<Ranking> lstRanking)
        {
            this.context = context;
            this.lstRanking = lstRanking;
        }

      
        public override int Count
        {
            get
            {
                return lstRanking.Count;
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {

          LayoutInflater inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
          View view = inflater.Inflate(Resource.Layout.row, null);
            TextView txtTop = view.FindViewById<TextView>(Resource.Id.txtTop);
            ImageView imgTop = view.FindViewById<ImageView>(Resource.Id.imgTop);

            if (position == 0)
                imgTop.SetBackgroundResource(Resource.Drawable.top1);
            else if (position == 1)
                imgTop.SetBackgroundResource(Resource.Drawable.top2);
            else
                imgTop.SetBackgroundResource(Resource.Drawable.top3);
            txtTop.Text = $"{ lstRanking[position].Score}";


            return view;
        }
    }
}