﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SurveyApp.Model;
using static Android.Views.View;

namespace SurveyApp
{
    [Activity(Label = "Play" , Theme="@style/AppTheme")]
    public class Play : Activity,IOnClickListener
    {
        const int INTERVAL = 1000;
        const int TIMEOUT = 6000;
        public int progressValue = 0;

        static CountDown mCountDown;
        List<Question> questionPlay = new List<Question>();
        DbHelper.DbHelper db;
        public static int index, score, thisQuestion, totalQuestion, correctAnswer;

        String mode = String.Empty;
       
        //Control
        public ProgressBar progressBar;
        ImageView imageView;
        Button btnA, btnB, btnC, btnD;
        TextView txtScore, txtQuestion;
      
        

        protected override void OnCreate(Bundle savedInstanceState)
        {
           base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Play);

            Bundle extra = Intent.Extras;
            if (extra != null)
                mode = extra.GetString("MODE");

            db = new DbHelper.DbHelper(this);
            txtScore = FindViewById<TextView>(Resource.Id.txtScore);
            txtQuestion = FindViewById<TextView>(Resource.Id.txtQuestion);
            progressBar = FindViewById<ProgressBar>(Resource.Id.progressBar);
            imageView = FindViewById<ImageView>(Resource.Id.question_flag);
            btnA = FindViewById<Button>(Resource.Id.btnAnswerA);
            btnB = FindViewById<Button>(Resource.Id.btnAnswerB);
            btnC = FindViewById<Button>(Resource.Id.btnAnswerC);
            btnD = FindViewById<Button>(Resource.Id.btnAnswerD);

            btnA.SetOnClickListener(this);
            btnB.SetOnClickListener(this);
            btnC.SetOnClickListener(this);
            btnD.SetOnClickListener(this);

        }

        public void OnClick(View v)
        {
            mCountDown.Cancel();
            if(index < totalQuestion)
            {
                Button btnClicked = (Button)v;
                if (btnClicked.Text.Equals(questionPlay[index].CorrectAnswer))
                {
                    score += 10;
                    correctAnswer++;
                    ShowQuestion(++index);

                }
                else 
                    ShowQuestion(++index);
            }
            txtScore.Text = $"{ score}";
        }

        private void ShowQuestion(int index)
        {
            if(index < totalQuestion)
            {
                thisQuestion++;
                txtQuestion.Text = $"{thisQuestion}/{totalQuestion}";
                progressBar.Progress = progressValue = 0;

                int ImageID = this.Resources.GetIdentifier(questionPlay[index].Image.ToLower(), "drawable", PackageName);
                imageView.SetBackgroundResource(ImageID);
                imageView.SetBackgroundResource(ImageID);
                btnA.Text = questionPlay[index].AnswerA;
                btnB.Text = questionPlay[index].AnswerB;
                btnC.Text = questionPlay[index].AnswerC;
                btnD.Text = questionPlay[index].AnswerD;

                mCountDown.Start();

            }
            else
            {
                Intent intent = new Intent(this, typeof(Done));
                Bundle dataSend = new Bundle();
                dataSend.PutInt("SCORE", score);
                dataSend.PutInt("TOTAL", totalQuestion);
                dataSend.PutInt("CORRECT", correctAnswer);

                intent.PutExtras(dataSend);
                StartActivity(intent);
                Finish();
            }
        }
        protected override void OnResume()
        {
            base.OnResume();
            index = 0;
            questionPlay = db.GetQuestionMode(mode);
            totalQuestion = questionPlay.Count;
            mCountDown = new CountDown(this, TIMEOUT, INTERVAL);
            ShowQuestion(index);
        }

         class CountDown : CountDownTimer
        {
            Play playing;



            public CountDown(Play playing, long totalTime, long interval) : base(totalTime, interval)
            {
                this.playing = playing;
            }

            public override void OnFinish()
            {
                Cancel();
                playing.ShowQuestion(++index);
            }

            public override void OnTick(long millisUntilFinished)
            {
                playing.progressValue++;
                playing.progressBar.Progress = playing.progressValue;
            }
        }


    }


}