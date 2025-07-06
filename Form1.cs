using System.Text.Json;
using NAudio.Wave;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Vosk;
using System.Speech.Synthesis;



namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        string tempKlasor = Application.StartupPath + @"\Temp";
        string videoDosyasi = "";
        string sesDosyasi = "";

        // EXE dosyalarının tam yolu, Application.StartupPath ile karışıklık olmasın diye tam yazdım:
        string ytDlpYolu = @"C:\Users\user\source\repos\WindowsFormsApp3\WindowsFormsApp3\voskTest\yt-dlp.exe";
        string ffmpegYolu = @"C:\Users\user\source\repos\WindowsFormsApp3\WindowsFormsApp3\voskTest\ffmpeg.exe";

        public Form1()
        {
            InitializeComponent();

            if (!Directory.Exists(tempKlasor))
                Directory.CreateDirectory(tempKlasor);

            lblDurum.Text = "Hazır";
        }

        private void btnCalistir_Click(object sender, EventArgs e)
        {
            string url = txtURL.Text;
            if (string.IsNullOrWhiteSpace(url))
            {
                MessageBox.Show("Lütfen geçerli bir YouTube URL'si girin.");
                return;
            }

            videoDosyasi = Path.Combine(tempKlasor, "video.mp4");
            sesDosyasi = Path.Combine(tempKlasor, "orijinal_ses.wav");

            try
            {
                lblDurum.Text = "Video indiriliyor...";
                Application.DoEvents();

                ProcessStartInfo psiYtdlp = new ProcessStartInfo
                {
                    FileName = ytDlpYolu,
                    Arguments = $"-f bestvideo+bestaudio --merge-output-format mp4 -o \"{videoDosyasi}\" {url}",
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                var procYtdlp = Process.Start(psiYtdlp);
                procYtdlp.WaitForExit();

                lblDurum.Text = "Video indirildi, ses ayıklanıyor...";
                Application.DoEvents();

                // Ses ayıklarken 16kHz mono olması çok önemli:
                ProcessStartInfo psiFfmpegSes = new ProcessStartInfo
                {
                    FileName = ffmpegYolu,
                    Arguments = $"-i \"{videoDosyasi}\" -vn -acodec pcm_s16le -ar 16000 -ac 1 \"{sesDosyasi}\"",
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                var procFfmpegSes = Process.Start(psiFfmpegSes);
                procFfmpegSes.WaitForExit();

                lblDurum.Text = "Ses ayıklandı, yazıya dökülüyor...";
                Application.DoEvents();

                // Model yolu tam yazıldı:
                string modelYolu = @"C:\Users\user\source\repos\WindowsFormsApp3\WindowsFormsApp3\voskTest\Model\vosk-model-small-tr-0.3\vosk-model-small-tr-0.3";

                string rawResult = VoskSpeechToText(sesDosyasi, modelYolu);

                // JSON içerisinden sadece text kısmını alıyoruz:
                string metin = ExtractTextFromVoskResult(rawResult);

                txtTranskript.Text = metin;

                lblDurum.Text = "Metin işleniyor, seslendiriliyor...";
                Application.DoEvents();

                string cevrilmisMetin = metin; // İstersen burada Google Translate kullanabilirsin

                string ttsDosyasi = Path.Combine(tempKlasor, "tts_ses.wav");
                MetniSeslendir(cevrilmisMetin, ttsDosyasi);

                lblDurum.Text = "Seslendirildi, video ile birleştiriliyor...";
                Application.DoEvents();

                string ciktiVideo = Path.Combine(tempKlasor, "sonuc_video.mp4");
                VideoVeSesBirlestir(videoDosyasi, ttsDosyasi, ciktiVideo);

                lblDurum.Text = "İşlem tamamlandı! Çıktı: " + ciktiVideo;
                MessageBox.Show("İşlem tamamlandı! Dosya: " + ciktiVideo);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
                lblDurum.Text = "Hata oluştu!";
            }
        }

        public string VoskSpeechToText(string sesDosyasi, string modelYolu)
        {
            try
            {
                Vosk.Vosk.SetLogLevel(0);
                StringBuilder sb = new StringBuilder();

                using (var model = new Model(modelYolu))
                using (var waveReader = new WaveFileReader(sesDosyasi))
                using (var rec = new VoskRecognizer(model, 16000))
                {
                    byte[] buffer = new byte[2048];
                    int bytesRead;

                    while ((bytesRead = waveReader.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        if (rec.AcceptWaveform(buffer, bytesRead))
                        {
                            var result = rec.Result();
                            sb.Append(result);
                        }
                    }
                    sb.Append(rec.FinalResult());
                }

                return sb.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Vosk HATASI: " + ex.ToString(), "HATA!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "";
            }
        }

        // Vosk'tan gelen JSON formatındaki sonucu sade metne çeviriyoruz
        public string ExtractTextFromVoskResult(string voskJson)
        {
            try
            {
                var jsonDoc = JsonDocument.Parse(voskJson);
                if (jsonDoc.RootElement.TryGetProperty("text", out var textProp))
                {
                    return textProp.GetString();
                }
            }
            catch
            {
                // JSON parse edilemezse orijinal string döner
            }
            return voskJson;
        }

        public void MetniSeslendir(string metin, string ciktiDosyasi)
        {
            using (SpeechSynthesizer synth = new SpeechSynthesizer())
            {
                synth.SelectVoiceByHints(VoiceGender.NotSet, VoiceAge.NotSet, 0, new System.Globalization.CultureInfo("tr-TR"));
                synth.SetOutputToWaveFile(ciktiDosyasi);
                synth.Speak(metin);
            }
        }

        public void VideoVeSesBirlestir(string videoDosyasi, string sesDosyasi, string ciktiDosyasi)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = ffmpegYolu,
                Arguments = $"-i \"{videoDosyasi}\" -i \"{sesDosyasi}\" -c:v copy -map 0:v:0 -map 1:a:0 -shortest \"{ciktiDosyasi}\"",
                UseShellExecute = false,
                CreateNoWindow = true
            };
            var proc = Process.Start(psi);
            proc.WaitForExit();
        }
    }
}


