using CommunityToolkit.Mvvm.ComponentModel;

using DomainService;

using System.ComponentModel;
using System.Windows;


namespace UiParts.UiWindow.MainWindow
{
    [INotifyPropertyChanged]
    public partial class MainWindowView
    {
        private Model _model;

        public int YYYVal
        {
            get => _model.AaaEntity.YYY.Value;
            set
            {
                if (value == YYYVal) return;

                var entity = _model.AaaEntity;
                entity.YYY = new(value);

                OnPropertyChanged();
            }
        }

        public int ZZZVal
        {
            get => _model.AaaEntity.ZZZ.Value;
            set
            {
                if (value == ZZZVal) return;

                var entity = _model.AaaEntity;

                if (entity.IsHaveToCorrectAAA(value))
                {
                    if (MessageBox.Show(
                        "ZZZ設定の変更によりAAA設定の設定値が補正され、それに関わる項目も補正される可能性があります。\n\nZZZ設定を変更しますか？",
                        "確認",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question,
                        MessageBoxResult.No) == MessageBoxResult.Yes)
                    {
                        entity.SetZZZ(new(value), new AAAChangedEvent(_model.AaaEntity, _model.BbbEntity));
                    }
                }
                else
                {
                    entity.SetZZZ(new(value), new AAAChangedEvent(_model.AaaEntity, _model.BbbEntity));
                }

                OnPropertyChanged();
                OnPropertyChanged(nameof(AAAVal));
                OnPropertyChanged(nameof(BBBVal));
            }
        }

        public int AAAVal
        {
            get => _model.AaaEntity.AAA.Value;
            set
            {
                if (value == AAAVal) return;

                var entity = _model.AaaEntity;

                if (entity.IsOverZZZ(value))
                {
                    MessageBox.Show("ZZZ設定を超える値を設定できません。");
                }
                else
                {
                    entity.SetAAA(new(value), new AAAChangedEvent(_model.AaaEntity, _model.BbbEntity));
                }

                OnPropertyChanged();
                OnPropertyChanged(nameof(BBBVal));
            }
        }

        public string BBBVal
        {
            get => _model.BbbEntity.BBB.Value;
            set
            {
                if (value == BBBVal) return;

                var text = value;

                var lengthChecker = new BBBLehgthChecker(_model.AaaEntity);
                if (!lengthChecker.IsValid(text))
                {
                    text = lengthChecker.Substring(text);
                }

                var entity = _model.BbbEntity;
                entity.SetBBB(new(text), lengthChecker);

                OnPropertyChanged();
            }
        }

        private void MainWindowViewModel(Model model)
        {
            _model = model;

            _model.PropertyChanged += Model_PropertyChanged;
        }

        private void Model_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_model.AaaEntity))
            {
                OnPropertyChanged(nameof(YYYVal));
                OnPropertyChanged(nameof(ZZZVal));
                OnPropertyChanged(nameof(AAAVal));
            }
            else if (e.PropertyName == nameof(_model.BbbEntity))
            {
                OnPropertyChanged(nameof(BBBVal));
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            _model.PropertyChanged -= Model_PropertyChanged;
        }
    }
}
