using Android.Content;
using Android.Icu.Text;
using Android.Preferences;
using Java.IO;
using Java.Sql;
using static Android.Renderscripts.Sampler;
using static Android.Telephony.CarrierConfigManager;

namespace mathScore
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private string[] numbers = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };
        private List<string> arithmetiñMean = new List<string>();

        private Button? onZeroButton, onOneButton, onTwoButton, onThreeButton, onFourButton, onFiveButton, onSixButton, onSevenButton, onEightButton, onNineButton, onTenButton;
        private Button? onClearButton, onClearAllButton;
        private TextView? onTextList, onTextCount, onTextResult;

        private ISharedPreferences? prefs;
        private ISharedPreferencesEditor? prefsEditor;
        private const string prefsKey = "mathScore.result";
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            onZeroButton = FindViewById<Button>(Resource.Id.zero);
            onZeroButton.Click += OnZeroButton_Click;
            onOneButton = FindViewById<Button>(Resource.Id.one);
            onOneButton.Click += onOneButton_Click;
            onTwoButton = FindViewById<Button>(Resource.Id.two);
            onTwoButton.Click += onTwoButton_Click;
            onThreeButton = FindViewById<Button>(Resource.Id.three);
            onThreeButton.Click += onThreeButton_Click;
            onFourButton = FindViewById<Button>(Resource.Id.four);
            onFourButton.Click += onFourButton_Click;
            onFiveButton = FindViewById<Button>(Resource.Id.five);
            onFiveButton.Click += onFiveButton_Click;
            onSixButton = FindViewById<Button>(Resource.Id.six);
            onSixButton.Click += onSixButton_Click;
            onSevenButton = FindViewById<Button>(Resource.Id.seven);
            onSevenButton.Click += onSevenButton_Click;
            onEightButton = FindViewById<Button>(Resource.Id.eight);
            onEightButton.Click += onEightButton_Click;
            onNineButton = FindViewById<Button>(Resource.Id.nine);
            onNineButton.Click += onNineButton_Click;
            onTenButton = FindViewById<Button>(Resource.Id.ten);
            onTenButton.Click += onTenButton_Click;

            onClearButton = FindViewById<Button>(Resource.Id.clear);
            onClearButton.Click += onClearButton_Click;
            onClearAllButton = FindViewById<Button>(Resource.Id.clearAll);
            onClearAllButton.Click += onClearAllButton_Click;

            onTextResult = FindViewById<TextView>(Resource.Id.textResult);
            onTextCount = FindViewById<TextView>(Resource.Id.textCount);
            onTextList = FindViewById<TextView>(Resource.Id.textList);

            Init();
        }

        private void OnZeroButton_Click(object? sender, EventArgs e) { addNumber("0"); }
        private void onOneButton_Click(object? sender, EventArgs e) { addNumber("1"); }
        private void onTwoButton_Click(object? sender, EventArgs e) { addNumber("2"); }
        private void onThreeButton_Click(object? sender, EventArgs e) { addNumber("3"); }
        private void onFourButton_Click(object? sender, EventArgs e) { addNumber("4"); }
        private void onFiveButton_Click(object? sender, EventArgs e) { addNumber("5"); }
        private void onSixButton_Click(object? sender, EventArgs e) { addNumber("6"); }
        private void onSevenButton_Click(object? sender, EventArgs e) { addNumber("7"); }
        private void onEightButton_Click(object? sender, EventArgs e) { addNumber("8"); }
        private void onNineButton_Click(object? sender, EventArgs e) { addNumber("9"); }
        private void onTenButton_Click(object? sender, EventArgs e) { addNumber("10"); }

        private void onClearButton_Click(object? sender, EventArgs e)
        {
            arithmetiñMean.Remove(arithmetiñMean[arithmetiñMean.Count - 1]);
            updateCount();
            updateList();
            updateResult();
        }

        private void onClearAllButton_Click(object? sender, EventArgs e) 
        {
            arithmetiñMean.Clear();
            updateCount();
            updateList();
            updateResult();
        }

        private void Init()
        {
            prefs = GetSharedPreferences("mathScore.result", FileCreationMode.Private);
            prefsEditor = prefs.Edit();

            using (var alert = new AlertDialog.Builder(this))
            {
                var title = "Âàø ïðåäûäóùèé ðåçóëüòàò: " + prefs.GetString(prefsKey, "-");
                alert.SetTitle(title);
                alert.SetPositiveButton("OK", (senderAlert, args) => { });
                alert.Create().Show();
            }
        }

        private void updateResult()
        {
            if (arithmetiñMean.Count < 1)
            {
                onTextResult.Text = "Ñð. àðèô.: -";
                onTextResult.SetTextColor(Android.Graphics.Color.DarkGray);
                return;
            }

            double sumElements = 0;

            foreach (string item in arithmetiñMean)
            {
                sumElements += double.Parse(item);
            }

            double result = Math.Round(sumElements / arithmetiñMean.Count, 2);

            if(result < 3) { onTextResult.SetTextColor(Android.Graphics.Color.DarkRed); }
            else if (result >= 3 && result <= 7) { onTextResult.SetTextColor(Android.Graphics.Color.Lime); }
            else if (result > 7 && result <= 10) { onTextResult.SetTextColor(Android.Graphics.Color.Blue); }
            else { onTextResult.SetTextColor(Android.Graphics.Color.DarkRed); }
            onTextResult.Text = "Ñð. àðèô.: " + result;

            prefsEditor.PutString(prefsKey, result.ToString());
            prefsEditor.Apply();
        }

        private void updateCount()
        {
            if (arithmetiñMean.Count < 1)
            {
                onTextCount.Text = "Êîëè÷åñòâî ÷èñåë: -";
                return;
            }

            onTextCount.Text = "Êîëè÷åñòâî ÷èñåë: " + arithmetiñMean.Count.ToString();
        }

        private void updateList()
        {
            if (arithmetiñMean.Count < 1)
            {
                onTextList.Text = "-";
                return;
            }

            string listText = string.Empty;
            int Index = 0;

            for (int i = 0; i != arithmetiñMean.Count; i++)
            {
                listText += arithmetiñMean[i] + " ";
                Index++;

                if (Index >= 13)
                {
                    Index = 0;
                    listText += "\n";
                }
            }

            onTextList.Text = listText;
        }

        private void addNumber(string number)
        {
            if (string.IsNullOrEmpty(number)) return;
            if (arithmetiñMean.Count >= 140) return;

            bool valid = false;
            foreach (string item in numbers)
            {
                if (number == item)
                {
                    valid = true;
                    break;
                }
            }

            if (valid) arithmetiñMean.Add(number);
            updateList();
            updateCount();
            updateResult();
        }
    }
}