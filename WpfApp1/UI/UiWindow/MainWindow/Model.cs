using CommunityToolkit.Mvvm.ComponentModel;
using Usecase;

namespace UiParts.UiWindow.MainWindow
{
    public partial class Model : ObservableObject
    {
        private readonly DisplaySettingsUsecase _displaySettingsUsecase;

        [ObservableProperty]
        private AAAEntity.AAAEntity _aaaEntity;

        [ObservableProperty]
        private BBBEntity.BBBEntity _bbbEntity;

        public Model(DisplaySettingsUsecase displaySettingsUsecase)
        {
            _displaySettingsUsecase = displaySettingsUsecase;

            AaaEntity = _displaySettingsUsecase.GetAAAEntity();
            BbbEntity = _displaySettingsUsecase.GetBBBEntity();
        }
    }
}
