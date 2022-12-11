using System.Media;

class ConfigBody
{
    public int MainVolume { get; set; }
    public int MusicVolume { get; set; }
    public int UIVolume { get; set; }
    public int SoundEffectVolume { get; set; }
    public bool fps_is_on { get; set; }
    public int fps_target { get; set; }
    public int display_index { get; set; }
    public bool vsync_is_on { get; set; }
    public int max_sec { get; set; }
    public int last_sec { get; set; }
}