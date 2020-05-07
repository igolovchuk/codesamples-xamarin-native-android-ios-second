
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using XamarinNativeDemo.Enums;
using XamarinNativeDemo.Extensions;
using XamarinNativeDemo.Models;
using Xamarin.RangeSlider;
using static Android.Widget.CompoundButton;

namespace XamarinNativeDemo.Droid.Activities
{
    public class RangeFragment : BaseDialogFragment
    {
        private RangeSliderControl _rangeSlider;
        private string _propertyValue;
        private float _minValue;
        private float _maxValue;
        private float _stepValue;
        private bool _doubleMode;
        private TexFormat _textFormat;

        public RangeFragment(string title, float minValue, float maxValue, float stepValue, string updateProperty, string propertyValue, TexFormat textFormat = TexFormat.F0)
            : base(title, updateProperty, Resource.Layout.RangeLayout)
        {
            _minValue = minValue;
            _maxValue = maxValue;
            _stepValue = stepValue;
            _propertyValue = propertyValue;
            _textFormat = textFormat;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            InitializeRangeSlider();
            InitializeSwitchSlider();
            InitializeSaveButton();

            return _baseView;
        }

       
        private void InitializeRangeSlider(){
            _rangeSlider = _baseView.FindViewById<RangeSliderControl>(Resource.Id.rangeSlider);

            if(_rangeSlider != null){
                _rangeSlider.SetRangeValues(_minValue, _maxValue);
                _rangeSlider.StepValue = _stepValue;

                _rangeSlider.ShowTextAboveThumbs = true;
                _rangeSlider.MinThumbHidden = true;
                _rangeSlider.MaxThumbHidden = false;

                _rangeSlider.ActivateOnDefaultValues = true;
                _rangeSlider.TextSizeInSp = 16;
                _rangeSlider.AlwaysActive = true;
               
                _rangeSlider.ThumbShadow = true;
                _rangeSlider.TextFormat = _textFormat.ToString();
                _rangeSlider.ActiveColor = Color.Rgb(46, 167, 243);//light blue
                _rangeSlider.TextAboveThumbsColor = Color.Rgb(219, 92, 76);//main
              
                SetSliderValues();
            }   
        }

        private void SetSliderValues()
        {
            float selectedMinValue = _minValue;
            float selectedMaxValue = CalculateAveragePosition();

            if (!string.IsNullOrEmpty(_propertyValue))
            {
                if (_propertyValue.Contains(Constants.SeparatorDash))
                {
                    var items = _propertyValue.Split(new[] { Constants.SeparatorDash }, StringSplitOptions.None);
                    selectedMinValue = items[0].ToFloat();
                    selectedMaxValue = items[1].ToFloat();
                    _doubleMode = true;
                }
                else
                {
                    selectedMaxValue = _propertyValue.ToFloat();
                }
            }

            ChangeValue(_doubleMode, selectedMinValue, selectedMaxValue);
        }

        private void ChangeValue(bool doubleModeOn, float selectedMinValue, float selectedMaxValue) 
        {
            _doubleMode = doubleModeOn;

            if (doubleModeOn)
            {
                _rangeSlider.DefaultColor = Color.Rgb(219, 92, 76);//main
                _rangeSlider.MinThumbHidden = false;
                _rangeSlider.SetSelectedMaxValue(selectedMaxValue);
                _rangeSlider.SetSelectedMinValue(selectedMinValue);
            }
            else
            {
                _rangeSlider.MinThumbHidden = true;
                _rangeSlider.DefaultColor = Color.Rgb(46, 167, 243);//light blue
                _rangeSlider.SetSelectedMaxValue(selectedMaxValue);
            }
        }

        // Ensure that this initialization is after RangeSlider initialization.
        private void InitializeSwitchSlider()
        {
            var switchSlider = _baseView.FindViewById<Switch>(Resource.Id.doubleModeSwitch);
            switchSlider.Checked = _doubleMode;
            switchSlider.CheckedChange += (object s, CheckedChangeEventArgs e) => {
                var maxValue = e.IsChecked ? _maxValue : CalculateAveragePosition();
                ChangeValue(e.IsChecked, _minValue, maxValue);
            };                               
        }

        private void InitializeSaveButton(){
            var btnSelectYear = _baseView.FindViewById<Button>(Resource.Id.btnSelectYear);
            btnSelectYear.Click += SelectYear;
        }

        private void SelectYear(object sender, EventArgs e)
        {
            string response;
            var selectedMinValue = _rangeSlider.GetSelectedMinValue();
            var selectedMaxValue = _rangeSlider.GetSelectedMaxValue();

            if (_doubleMode && Math.Abs(selectedMaxValue - selectedMinValue) > 0)
            {
                response = $"{selectedMinValue}{Constants.SeparatorDash}{selectedMaxValue}";
            }
            else
            {
                response = selectedMaxValue.ToString();
            }

            InvokeResultReceived(new NameValueItem { Name = response, Value = response });
            Close();
        }

        private float CalculateAveragePosition() {
            float selectedMaxValue = _minValue + (float)(Math.Round(((_maxValue - _minValue) / 2), 1));

            if (_textFormat == TexFormat.F0)
            {
                selectedMaxValue = (int)selectedMaxValue;
            }

            return selectedMaxValue;
        }
    }
}
