namespace WpfSpeechOnCoreWithUWPApi
{
    public class SelectGender : NotifyObject
    {
        private Windows.Media.SpeechSynthesis.VoiceGender _selectedGender;
        public Windows.Media.SpeechSynthesis.VoiceGender SelectedGender
        {
            get { return _selectedGender; }
            set { base.SetProperty(ref _selectedGender, value); }
        }
    }
}
