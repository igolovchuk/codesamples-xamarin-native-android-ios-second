using Android.App;
using Android.Widget;
using Android.OS;
using XamarinNativeDemo.Models;
using System.Collections.Generic;
using XamarinNativeDemo.Interfaces;
using XamarinNativeDemo.Providers;
using Unity.Attributes;
using Unity;
using System.Threading.Tasks;
using System.Linq;
using System;
using XamarinNativeDemo.Enums;
using XamarinNativeDemo.Droid.Providers;
using XamarinNativeDemo.Extensions;
using XamarinNativeDemo.Droid.Extensions;
using Android.Mtp;
using Android.Views;
using Android.Text;

namespace XamarinNativeDemo.Droid.Activities
{
    [Activity(Icon = "@mipmap/icon", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MainActivity : Activity
    {
        private TableRow _marksRow;
        private TableRow _modelsRow;
        private TableRow _yearsRow;
        private TableRow _engineVolumeRow;
        private TableRow _gearBoxRow;
        private TableRow _fuelTypeRow;
        private TableRow _customsClearedRow;
        private Button _calculationButton;
       
        private List<NameValueItem> _markList;
        private List<NameValueItem> _modelList;
        //private List<NameValueItem> _yearList;
        private List<NameValueItem> _engineVolumeList;
        private List<NameValueItem> _gearBoxList;
        private List<NameValueItem> _fuelTypeList;

        private PickerFragment _pickerFragment;
        private RangeFragment _rangeFragment;
        private Vehicle _vehicle;

        [Dependency]
        public ICarDataProvider CarDataProvider { get;set;}

        [Dependency]
        public IStatisticDataProvider StatisticDataProvider { get; set; }

        [Dependency]
        public INetworkProvider NetworkProvider { get; set; }

        public MainActivity()
        {
            UnityContainerProvider.Container.BuildUp(this);

            if(_vehicle == null){
                _vehicle = new Vehicle();
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            InitializeControls();

            LoadData();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.right_menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.item_menu_refresh:
                    LoadData();
                    break;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void PickerFragment_ResultReceived(string updateProperty, NameValueItem nameValueItem)
        {
            UpdateProperty(updateProperty, nameValueItem);

            CheckAvailability();
        }

        private void LoadMarkList()
        {
            _marksRow.SetLoadingState();
           
            Task.Run(async () => _markList = _markList ?? await CarDataProvider.GetMarkList())
                .ContinueWith(_ => EnableControl(_marksRow, _markList));
        }

        private void LoadModelList(string markId)
        {
            _modelsRow.SetLoadingState();

            Task.Run(async () => _modelList = _modelList ?? await CarDataProvider.GetModelList(markId))
                .ContinueWith(_ => EnableControl(_modelsRow, _modelList));
        }

        private void LoadYearList()
        {
             _yearsRow.SetLoadingState();

            // Task.Run(async () => _yearList = _yearList ?? await CarDataProvider.GetYearList())
            //    .ContinueWith(_ => EnableControl(_yearsRow, _yearList));
            _vehicle.Year = null;
            _yearsRow.SetActiveState(true);
        }

        private void LoadEngineVolumeList()
        {
           _engineVolumeRow.SetLoadingState();

            Task.Run(async () => _engineVolumeList = _engineVolumeList ?? await CarDataProvider.GetEngineVolumeList())
                .ContinueWith(_ => EnableControl(_engineVolumeRow, _engineVolumeList, true, nameof(_vehicle.EngineVolume)));
        }

        private void LoadGearBoxList()
        {
            _gearBoxRow.SetLoadingState();
         
            Task.Run(async () => {
                if(_gearBoxList == null){
                    var result = await CarDataProvider.GetGearBoxList();
                    _gearBoxList = result.Select(x =>
                    new NameValueItem
                    {
                        Name = LocalizationProvider.Translate(x.Name
                                                              .ToLower().Replace(Constants.SeparatorForwardWithSpaces,
                                                                                 Constants.SeparatorUnderscore)),
                        Value = x.Value
                    }).ToList(); 
                 }
                })
                .ContinueWith(_ => EnableControl(_gearBoxRow, _gearBoxList, true, nameof(_vehicle.GearId)));
        }

        private void LoadCustomsClearedValue(){
            var btnCustomsCleared = _customsClearedRow.FindViewById<ToggleButton>(Resource.Id.btnCustomsCleared);
            btnCustomsCleared.Checked = false;
            _vehicle.IsNotLegalized = Convert.ToInt32(btnCustomsCleared.Checked).ToString();
        }

        private void LoadFuelTypeList()
        {
            _fuelTypeRow.SetLoadingState();

            Task.Run(async () => {
                if(_fuelTypeList == null){
                    var result = await CarDataProvider.GetFuelTypeList();
                    _fuelTypeList = result.Select(x =>
                    new NameValueItem
                    {
                        Name = LocalizationProvider.Translate(x.Name.ToLower()
                                                   .Replace(new char[] { '/', '-', ' ' }, Constants.SeparatorUnderscore)),
                        Value = x.Value
                    }).ToList();
                }     
               })
                .ContinueWith(_ => EnableControl(_fuelTypeRow, _fuelTypeList, true, nameof(_vehicle.FuelId)));
        }

        private void ShowPopup(string title, List<NameValueItem> model, string propertyName, InputTypes inputType = InputTypes.ClassText)
        {
            _pickerFragment = new PickerFragment(title, model, propertyName, inputType);
            _pickerFragment.ResultReceived += PickerFragment_ResultReceived;
            _pickerFragment.Show(FragmentManager, nameof(Fragment));
        }

        private void ShowRange(int titleResource, int minValue, int maxValue, int stepValue, string updateProperty, string propertyValue)
        {
            _rangeFragment = new RangeFragment(LocalizationProvider.Translate(titleResource), minValue, maxValue, stepValue, updateProperty, propertyValue);
            _rangeFragment.ResultReceived += PickerFragment_ResultReceived;
            _rangeFragment.Show(FragmentManager, nameof(RangeFragment));
        }

        private void InitializeControls(){
            InitializeActionBar();

            /// Models button initialization.
            _modelsRow = FindViewById<TableRow>(Resource.Id.rowModels);
            _modelsRow.Click += (sender, e) => ShowData(Resource.String.select_model, _modelList, nameof(_vehicle.ModelId), (TableRow)sender);
           
            // Marks button initialization.
            _marksRow = FindViewById<TableRow>(Resource.Id.rowMarks);
            _marksRow.Click += (sender, e) =>
            {
                ShowData(Resource.String.select_mark, _markList, nameof(_vehicle.Id), (TableRow)sender);
            };

            _yearsRow = FindViewById<TableRow>(Resource.Id.rowYear);
            _yearsRow.Click += (sender, e) =>
            {
                ShowRange(Resource.String.select_year, DateTime.Now.Year - Constants.TimeSlotInYears, DateTime.Now.Year, 1, nameof(_vehicle.Year), _vehicle.Year);
            };
            // _yearsRow.Click += (sender, e) => ShowData(Resource.String.select_year, _yearList, nameof(_vehicle.Year), (TableRow)sender, InputTypes.ClassNumber);

            _engineVolumeRow = FindViewById<TableRow>(Resource.Id.rowEngineVolume);
            _engineVolumeRow.Click += (sender, e) => ShowData(Resource.String.engine_volume, _engineVolumeList, nameof(_vehicle.EngineVolume), (TableRow)sender, InputTypes.ClassNumber | InputTypes.NumberFlagDecimal);

            _gearBoxRow = FindViewById<TableRow>(Resource.Id.rowGearBox);
            _gearBoxRow.Click += (sender, e) => ShowData(Resource.String.gear_box_type, _gearBoxList, nameof(_vehicle.GearId), (TableRow)sender);

            _fuelTypeRow = FindViewById<TableRow>(Resource.Id.rowFuelType);
            _fuelTypeRow.Click += (sender, e) => ShowData(Resource.String.fuel_type, _fuelTypeList, nameof(_vehicle.FuelId), (TableRow)sender);

            _customsClearedRow = FindViewById<TableRow>(Resource.Id.rowCustomsCleared);
            var customsClearedButton = _customsClearedRow.FindViewById<ToggleButton>(Resource.Id.btnCustomsCleared);
            customsClearedButton.Click += (sender, e) =>
            {
                _vehicle.IsNotLegalized = Convert.ToInt32(_customsClearedRow.FindViewById<ToggleButton>(Resource.Id.btnCustomsCleared).Checked).ToString();
            };
            _customsClearedRow.Click += (sender, e) => customsClearedButton.PerformClick();

            _calculationButton = FindViewById<Button>(Resource.Id.btnCalculate);
            _calculationButton.Click += Calculate;
        }

        private void CheckAvailability() {
            if(!string.IsNullOrEmpty(_vehicle.Id)
               && !string.IsNullOrEmpty(_vehicle.ModelId)
               && !string.IsNullOrEmpty(_vehicle.Year)
               && !string.IsNullOrEmpty(_vehicle.GearId)
               && !string.IsNullOrEmpty(_vehicle.FuelId)
               && !string.IsNullOrEmpty(_vehicle.EngineVolume)
               && !string.IsNullOrEmpty(_vehicle.IsNotLegalized)){
                _calculationButton.SetSate(true);
            } else {
                _calculationButton.SetSate(false);
            }
        }

        private void ShowData(int titleResource, List<NameValueItem> dataList, string propertyName, TableRow sender, InputTypes inputType = InputTypes.ClassText){
            if(sender.IsEnabled()){
                if (dataList != null)
                {
                    ShowPopup(LocalizationProvider.Translate(titleResource), dataList, propertyName, inputType);
                }
                else
                {
                    this.ShowError(LocalizationProvider.Translate(Resource.String.message_refresh));
                }  
            } 
        }

        private void EnableControl(TableRow tableRow, List<NameValueItem> dataList, bool setDefaultValue = false, string propertyToUpdate = null)
        {
            RunOnUiThread(() =>
            {
                if (dataList != null && setDefaultValue && !string.IsNullOrEmpty(propertyToUpdate))
                {
                   dataList.Insert(0, new NameValueItem
                   {
                       Name = LocalizationProvider.Translate(Resource.String.message_not_include),
                       Value = Constants.NoParamValue
                   });

                   UpdateProperty(propertyToUpdate, dataList[0]);
                }
                else {
                    tableRow.SetActiveState(dataList != null);
                }
            });
        }

        private async void Calculate(object sender, EventArgs e)
        {
            var calculateBtn = (Button)sender;
            calculateBtn.SetSate(false);

            var dialog = this.ShowProgress();

            var data = await CarDataProvider.Average(_vehicle);
            dialog.Dismiss();

            if(data != null){
                if (data.ArithmeticMean != decimal.Zero)
                {
                    var carName = $"{_marksRow.FindViewById<TextView>(Resource.Id.txtMark).Text} {_modelsRow.FindViewById<TextView>(Resource.Id.txtModel).Text}";
                    var customsCleared = _customsClearedRow.FindViewById<ToggleButton>(Resource.Id.btnCustomsCleared).Checked 
                                         ? Resource.String.no 
                                         : Resource.String.yes;

                    var message = string.Format(LocalizationProvider.Translate(Resource.String.result_template),
                                                carName, _yearsRow.FindViewById<TextView>(Resource.Id.txtYear).Text,
                                                _engineVolumeRow.FindViewById<TextView>(Resource.Id.txtEngineVolume).Text,
                                                _gearBoxRow.FindViewById<TextView>(Resource.Id.txtGearBox).Text,
                                                _fuelTypeRow.FindViewById<TextView>(Resource.Id.txtFuelType).Text,
                                                LocalizationProvider.Translate(customsCleared),
                                                data.Total, (int)data.ArithmeticMean);

                    StatisticDataProvider.SendAsync(Projects.Auto, message);

                    this.ShowDialog(LocalizationProvider.Translate(Resource.String.result_title), message);
                }
                else
                {
                    this.ShowError(LocalizationProvider.Translate(Resource.String.result_not_found));
                }
            }
            else {
                this.ShowError(LocalizationProvider.Translate(Resource.String.result_error));
            }

            calculateBtn.SetSate(true);
        }

        private void InitializeActionBar() {
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            toolbar.SetNavigationIcon(Resource.Mipmap.close);
            SetActionBar(toolbar);

            toolbar.NavigationOnClick += (s, e) => FinishAndRemoveTask();
        }

        private void LoadData() {
            if (NetworkProvider.IsConnected())
            {
                LoadMarkList();
                LoadYearList();
                LoadEngineVolumeList();
                LoadGearBoxList();
                LoadCustomsClearedValue();
                LoadFuelTypeList();
                _modelList = null;
                _modelsRow.SetData(false, Resource.String.select);

                _calculationButton.Enabled = false;
                _calculationButton.SetBackgroundResource(Resource.Color.light_grey);
            }
            else
            {
                this.ShowDialog(LocalizationProvider.Translate(Resource.String.title_error),
                                LocalizationProvider.Translate(Resource.String.message_no_connection),
                                FinishAndRemoveTask, Resource.String.exit);
            }
        }

        private void UpdateProperty(string propertyName, NameValueItem nameValueItem)
        {
            switch (propertyName)
            {
                case nameof(_vehicle.Id):
                    _vehicle.Id = nameValueItem.Value;
                    _vehicle.ModelId = string.Empty;
                    _modelList = null;
                    _marksRow.SetTitle(nameValueItem.Name);
                    _modelsRow.FindViewById<TextView>(Resource.Id.txtModel).Text = LocalizationProvider.Translate(Resource.String.message_loading);
                    LoadModelList(_vehicle.Id);
                    break;
                case nameof(_vehicle.ModelId):
                    _vehicle.ModelId = nameValueItem.Value;
                    _modelsRow.SetTitle(nameValueItem.Name);
                    break;
                case nameof(_vehicle.Year):
                    _vehicle.Year = nameValueItem.Value;
                    _yearsRow.SetTitle(nameValueItem.Name);
                    break;
                case nameof(_vehicle.FuelId):
                    _vehicle.FuelId = nameValueItem.Value;
                    _fuelTypeRow.SetTitle(nameValueItem.Name);
                    break;
                case nameof(_vehicle.EngineVolume):
                    _vehicle.EngineVolume = nameValueItem.Value;
                    _engineVolumeRow.SetTitle(nameValueItem.Name);
                    break;
                case nameof(_vehicle.GearId):
                    _vehicle.GearId = nameValueItem.Value;
                    _gearBoxRow.SetTitle(nameValueItem.Name);
                    break;
            }
        }
    }
}

