using HospitalChargesApp.Annotations;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace HospitalChargesApp
{
    /// <summary>
    /// Allows having string and digit representations of the number.
    /// When the Text property is set, automatically in the Value property, the decimal equivalent to the Text value will be placed. 
    /// </summary>
    public class StringNumber
    {
        private readonly bool _intValue;
        private readonly string _stringFormat;

        private string _text;
        public string Text
        {
            get => _text;
            set
            {
                if (decimal.TryParse(value, out var result))
                {
                    Value = _intValue ? Math.Ceiling(result) : result;
                    _text = Value.ToString(_stringFormat);
                    return;
                }

                Value = 0;
                _text = Value.ToString(_stringFormat);
            }
        }
        public decimal Value { get; private set; }

        /// <summary></summary>
        /// <param name="intValue">If true, Math.Ceil will be applied for the given fractional number (e.g. 3.1 -> 4)</param>
        /// <param name="stringFormat">String display format</param>
        public StringNumber(bool intValue = false, string stringFormat = "0.00")
        {
            _intValue = intValue;
            _stringFormat = stringFormat;

            Text = "0";
        }
    }

    public class ViewModel : INotifyPropertyChanged
    {
        private const int BASE_STAY_CHARGE = 350;
        private const int NUMBER_OF_CHARGES = 5;

        private decimal _totalCharges;
        public decimal TotalCharges
        {
            get => _totalCharges;
            set { _totalCharges = value; OnPropertyChanged(); }
        }

        private StringNumber _daysNumber = new StringNumber(true, "0");
        public StringNumber DaysNumber
        {
            get => _daysNumber;
            set { _daysNumber = value; OnPropertyChanged(); }
        }

        private BindingList<StringNumber> _miscCharges;
        public BindingList<StringNumber> MiscCharges
        {
            get => _miscCharges;
            set { _miscCharges = value; OnPropertyChanged(); }
        }

        public ViewModel()
        {
            MiscCharges = new BindingList<StringNumber> { AllowEdit = true, RaiseListChangedEvents = true };

            for (var i = 0; i < NUMBER_OF_CHARGES; i++)
                MiscCharges.Add(new StringNumber());
        }

        public decimal GetMiscCharges()
        {
            return MiscCharges.Sum(c => c.Value);
        }

        public decimal GetStayCharges()
        {
            return BASE_STAY_CHARGE * DaysNumber.Value;
        }

        public void CalcTotalCharges()
        {
            TotalCharges = GetStayCharges() + GetMiscCharges();
        }

        public void Clear()
        {
            DaysNumber.Text = "0";
            TotalCharges = 0;
            foreach (var charge in MiscCharges) charge.Text = "0";

            // We do this in order to call OnPropertyChanged() for these properties
            DaysNumber = DaysNumber;
            MiscCharges = MiscCharges;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}