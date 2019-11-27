using System;
using System.Collections.Generic;
using System.Drawing;

namespace Novych.Models
{
    public enum Tone
    {
        G1, A1, H1, C2, D2, E2, FIS2, G2, A2, H2, C3, D3, E3, FIS3, G3,
        G1_, A1_, H1_, C2_, D2_, E2_, FIS2_, G2_, A2_, H2_, C3_, D3_, E3_, FIS3_, G3_,
        X
    }

    public class Note
    {
        public Tone ton;
        public String name;
        public float y_pos;
        public float x_pos;
        public bool long_ton;

        public Note(String name, Tone tone, float y_pos, float x_pos, bool long_ton = false)
        {
            this.ton = tone;
            this.name = name;
            this.y_pos = y_pos + 5;
            this.x_pos = x_pos + 2;
            this.long_ton = long_ton;
        }
    }

    class Citerka
    {
        private bool DRAW_TONE_LINES;
        private bool FIX_PRINTER_MARGIN;

        private int BOTTOM_OFFSET;
        private float X_OFFSET;

        private int PICTURE_WIDTH_PX;
        private int PICTURE_WIDTH;
        private float PICTURE_WIDTH_MM;

        private int PICTURE_HEIGHT_PX;
        private int PICTURE_HEIGHT;
        private float PICTURE_HEIGHT_MM;

        private float LINE_WIDTH;
        private float LINE_WIDTH_BOLD;
        private int STRINGS_HIGHT;

        private int TONE_WIDTH;
        private int TONE_HEIGHT;
        private int SONG_X_OFFSET;

        private int HEADER_TEXT_POS;
        private int FOOTER_TEXT_POS;

        public static int HEADER_TEXT_MAX_LEN = 20;
        public static int FOOTER_TEXT_MAX_LEN = 50;

        private Bitmap bitmap;
        private Graphics g;

        private Pen redPenBold;
        private Pen blackPen;
        private Pen blackPenBold;

        private Brush whiteBrush;
        private Brush redBrush;
        private Brush blackBrush;

        private Font toneFont;
        private Font headerTextFont;
        private Font footerTextFont;

        public Dictionary<Tone, Note> tonesDef { get; set; }

        public static List<Tone> allToneList = new List<Tone> {Tone.G1, Tone.A1, Tone.H1, Tone.C2, Tone.D2, Tone.E2, Tone.FIS2, Tone.G2,
                                                         Tone.A2, Tone.H2, Tone.C3, Tone.D3, Tone.E3, Tone.FIS3, Tone.G3,
                                                         Tone.G1_, Tone.A1_, Tone.H1_, Tone.C2_, Tone.D2_, Tone.E2_, Tone.FIS2_, Tone.G2_,
                                                         Tone.A2_, Tone.H2_, Tone.C3_, Tone.D3_, Tone.E3_, Tone.FIS3_, Tone.G3_, Tone.X };

        private static List<Tone> citerkaToneList = new List<Tone> {Tone.G1, Tone.A1, Tone.H1, Tone.C2, Tone.D2, Tone.E2, Tone.FIS2, Tone.G2,
                                                      Tone.A2, Tone.H2, Tone.C3, Tone.D3, Tone.E3, Tone.FIS3, Tone.G3 };
        private static List<int> tones_Y_pos = new List<int> { 13, 26, 40, 51, 64, 76, 90, 101, 114, 126, 140, 151, 164, 176, 190 };

        public string logBuffer;

        public Citerka(int width = 1123, int height = 794, bool draw_tone_lines = false, bool fix_printer_margin = false)
        {
            DRAW_TONE_LINES = draw_tone_lines;
            FIX_PRINTER_MARGIN = fix_printer_margin;

            BOTTOM_OFFSET = 5;
            X_OFFSET = 6.25f;

            PICTURE_WIDTH_PX = width;
            PICTURE_WIDTH = 297;
            PICTURE_WIDTH_MM = (float)PICTURE_WIDTH_PX / PICTURE_WIDTH;

            PICTURE_HEIGHT_PX = height;
            PICTURE_HEIGHT = 210;
            PICTURE_HEIGHT_MM = (float)PICTURE_HEIGHT_PX / PICTURE_HEIGHT;

            LINE_WIDTH = PICTURE_WIDTH_MM / 4;
            LINE_WIDTH_BOLD = PICTURE_WIDTH_MM / 2;
            STRINGS_HIGHT = (int)(PICTURE_HEIGHT_MM * 4);

            SONG_X_OFFSET = 20;
            HEADER_TEXT_POS = 190 + BOTTOM_OFFSET;
            FOOTER_TEXT_POS = 20 + BOTTOM_OFFSET;

            redPenBold = new Pen(Color.Red, LINE_WIDTH_BOLD);
            blackPen = new Pen(Color.Black, LINE_WIDTH);
            blackPenBold = new Pen(Color.Black, LINE_WIDTH_BOLD);

            whiteBrush = new SolidBrush(Color.White);
            redBrush = new SolidBrush(Color.Red);
            blackBrush = new SolidBrush(Color.Black);

            toneFont = new Font(FontFamily.GenericSansSerif, STRINGS_HIGHT, FontStyle.Regular);
            headerTextFont = new Font(FontFamily.GenericSansSerif, STRINGS_HIGHT * 2, FontStyle.Regular);
            footerTextFont = new Font(FontFamily.GenericSansSerif, STRINGS_HIGHT * 2, FontStyle.Regular);

            initializeNotes();

            drawBackground();
        }

        public Image getImage()
        {
            return bitmap;
        }

        public Image clear()
        {
            drawBackground();

            return bitmap;
        }

        public Image drawSong(List<Tone> song, string headerString, string footerString)
        {
            //log("drawSong");

            drawBackground();


            int count = song.Count;

            if (headerString != null && headerString.Length > 0)
            {
                SizeF size = g.MeasureString(headerString, headerTextFont);
                drawString(headerString, headerTextFont, blackBrush, PICTURE_WIDTH / 2 - (0.97f * size.Width / 2 / PICTURE_WIDTH_MM), HEADER_TEXT_POS);
            }
            else if (headerString != null && headerString.Length > HEADER_TEXT_MAX_LEN)
            {
                log("Maximum Header Text length [" + HEADER_TEXT_MAX_LEN + "] exceeded");
                throw new Exception("Překročena maximální délka Názvu: " + HEADER_TEXT_MAX_LEN + " znaků");
            }

            if (footerString != null && footerString.Length > 0)
            {
                SizeF size = g.MeasureString(footerString, footerTextFont);
                drawString(footerString, footerTextFont, blackBrush, PICTURE_WIDTH / 2 - (0.97f * size.Width / 2 / PICTURE_WIDTH_MM), FOOTER_TEXT_POS);
            }
            else if (footerString != null && footerString.Length > FOOTER_TEXT_MAX_LEN)
            {
                log("Maximum Footer Text length [" + FOOTER_TEXT_MAX_LEN + "] exceeded");
                throw new Exception("Překročena maximální délka Popisu: " + FOOTER_TEXT_MAX_LEN + " znaků");
            }

            if (count <= 0)
            {
                log("No notes inserted");

                return bitmap;
            }

            if (count < 25)
            {
                TONE_WIDTH = 6;
                TONE_HEIGHT = 4;
            }
            else if (count <= 60)
            {
                TONE_WIDTH = 2;
                TONE_HEIGHT = 4;
            }
            else
            {
                log("Maximum notes [60] exceeded");

                throw new Exception("Překročen maximální počet Not: " + 60 + " not");
            }


            Note firstNote = tonesDef[song[0]];
            Note lastNote = tonesDef[song[count - 1]];

            float x_start = firstNote.x_pos + SONG_X_OFFSET;
            float x_end = PICTURE_WIDTH - lastNote.x_pos - SONG_X_OFFSET;
            float x_off = (x_end - x_start) / (count - 1);

            Note nextNote = null;
            float nextX = 0;

            float x_curr = x_start;
            for (int i = 0; i < count; i++)
            {
                if (song[i] == Tone.X)
                {
                    continue;
                }

                Note currNote = tonesDef[song[i]];

                if (i < count - 1)
                {
                    if (song[i + 1] == Tone.X)
                    {
                        nextX = 0;
                    }
                    else
                    {
                        nextNote = tonesDef[song[i + 1]];
                        nextX = x_curr + x_off;
                    }
                }

                if (nextX > 0)
                {
                    drawLine(blackPen, nextX, nextNote.y_pos, x_curr, currNote.y_pos);
                }

                drawNote(currNote, x_curr);

                x_curr += x_off;
            }

            return bitmap;
        }

        private void log(String msg)
        {

            logBuffer += DateTime.Now.ToString("[hh:mm:ss.fff]") + " [Citerka]   " + msg + "\n";
        }

        private void initializeNotes()
        {
            //log("initializeNotes");

            int i = 0;

            tonesDef = new Dictionary<Tone, Note>();

            if (FIX_PRINTER_MARGIN)
            {
                tonesDef.Add(Tone.G1, new Note("g1", Tone.G1, tones_Y_pos[i], X_OFFSET * i + 25)); i++;
                tonesDef.Add(Tone.A1, new Note("a1", Tone.A1, tones_Y_pos[i], X_OFFSET * i + 20)); i++;
                tonesDef.Add(Tone.H1, new Note("h1", Tone.H1, tones_Y_pos[i], X_OFFSET * i + 15)); i++;
                tonesDef.Add(Tone.C2, new Note("c2", Tone.C2, tones_Y_pos[i], X_OFFSET * i + 10)); i++;
                tonesDef.Add(Tone.D2, new Note("d2", Tone.D2, tones_Y_pos[i], X_OFFSET * i + 5)); i++;
            }
            else
            {
                tonesDef.Add(Tone.G1, new Note("g1", Tone.G1, tones_Y_pos[i], X_OFFSET * i)); i++;
                tonesDef.Add(Tone.A1, new Note("a1", Tone.A1, tones_Y_pos[i], X_OFFSET * i)); i++;
                tonesDef.Add(Tone.H1, new Note("h1", Tone.H1, tones_Y_pos[i], X_OFFSET * i)); i++;
                tonesDef.Add(Tone.C2, new Note("c2", Tone.C2, tones_Y_pos[i], X_OFFSET * i)); i++;
                tonesDef.Add(Tone.D2, new Note("d2", Tone.D2, tones_Y_pos[i], X_OFFSET * i)); i++;
            }

            tonesDef.Add(Tone.E2, new Note("e2", Tone.E2, tones_Y_pos[i], X_OFFSET * i)); i++;
            tonesDef.Add(Tone.FIS2, new Note("fis2", Tone.FIS2, tones_Y_pos[i], X_OFFSET * i)); i++;
            tonesDef.Add(Tone.G2, new Note("g2", Tone.G2, tones_Y_pos[i], X_OFFSET * i)); i++;
            tonesDef.Add(Tone.A2, new Note("a2", Tone.A2, tones_Y_pos[i], X_OFFSET * i)); i++;
            tonesDef.Add(Tone.H2, new Note("h2", Tone.H2, tones_Y_pos[i], X_OFFSET * i)); i++;
            tonesDef.Add(Tone.C3, new Note("c3", Tone.C3, tones_Y_pos[i], X_OFFSET * i)); i++;
            tonesDef.Add(Tone.D3, new Note("d3", Tone.D3, tones_Y_pos[i], X_OFFSET * i)); i++;
            tonesDef.Add(Tone.E3, new Note("e3", Tone.E3, tones_Y_pos[i], X_OFFSET * i)); i++;
            tonesDef.Add(Tone.FIS3, new Note("fis3", Tone.FIS3, tones_Y_pos[i], X_OFFSET * i)); i++;
            tonesDef.Add(Tone.G3, new Note("g3", Tone.G3, tones_Y_pos[i], X_OFFSET * i)); i++;

            i = 0;
            if (FIX_PRINTER_MARGIN)
            {
                tonesDef.Add(Tone.G1_, new Note("g1_", Tone.G1_, tones_Y_pos[i], X_OFFSET * i + 25, true)); i++;
                tonesDef.Add(Tone.A1_, new Note("a1_", Tone.A1_, tones_Y_pos[i], X_OFFSET * i + 20, true)); i++;
                tonesDef.Add(Tone.H1_, new Note("h1_", Tone.H1_, tones_Y_pos[i], X_OFFSET * i + 15, true)); i++;
                tonesDef.Add(Tone.C2_, new Note("c2_", Tone.C2_, tones_Y_pos[i], X_OFFSET * i + 10, true)); i++;
                tonesDef.Add(Tone.D2_, new Note("d2_", Tone.D2_, tones_Y_pos[i], X_OFFSET * i + 5, true)); i++;
            }
            else
            {
                tonesDef.Add(Tone.G1_, new Note("g1_", Tone.G1_, tones_Y_pos[i], X_OFFSET * i, true)); i++;
                tonesDef.Add(Tone.A1_, new Note("a1_", Tone.A1_, tones_Y_pos[i], X_OFFSET * i, true)); i++;
                tonesDef.Add(Tone.H1_, new Note("h1_", Tone.H1_, tones_Y_pos[i], X_OFFSET * i, true)); i++;
                tonesDef.Add(Tone.C2_, new Note("c2_", Tone.C2_, tones_Y_pos[i], X_OFFSET * i, true)); i++;
                tonesDef.Add(Tone.D2_, new Note("d2_", Tone.D2_, tones_Y_pos[i], X_OFFSET * i, true)); i++;
            }

            tonesDef.Add(Tone.E2_, new Note("e2_", Tone.E2_, tones_Y_pos[i], X_OFFSET * i, true)); i++;
            tonesDef.Add(Tone.FIS2_, new Note("fis2_", Tone.FIS2_, tones_Y_pos[i], X_OFFSET * i, true)); i++;
            tonesDef.Add(Tone.G2_, new Note("g2_", Tone.G2_, tones_Y_pos[i], X_OFFSET * i, true)); i++;
            tonesDef.Add(Tone.A2_, new Note("a2_", Tone.A2_, tones_Y_pos[i], X_OFFSET * i, true)); i++;
            tonesDef.Add(Tone.H2_, new Note("h2_", Tone.H2_, tones_Y_pos[i], X_OFFSET * i, true)); i++;
            tonesDef.Add(Tone.C3_, new Note("c3_", Tone.C3_, tones_Y_pos[i], X_OFFSET * i, true)); i++;
            tonesDef.Add(Tone.D3_, new Note("d3_", Tone.D3_, tones_Y_pos[i], X_OFFSET * i, true)); i++;
            tonesDef.Add(Tone.E3_, new Note("e3_", Tone.E3_, tones_Y_pos[i], X_OFFSET * i, true)); i++;
            tonesDef.Add(Tone.FIS3_, new Note("fis3_", Tone.FIS3_, tones_Y_pos[i], X_OFFSET * i, true)); i++;
            tonesDef.Add(Tone.G3_, new Note("g3_", Tone.G3_, tones_Y_pos[i], X_OFFSET * i, true)); i++;
        }

        private void drawBackground()
        {
            //log("drawBackground [" + PICTURE_WIDTH + "," + PICTURE_HEIGHT + "] [" + PICTURE_WIDTH_PX + "," + PICTURE_HEIGHT_PX + "]");

            bitmap = new Bitmap(PICTURE_WIDTH_PX, PICTURE_HEIGHT_PX);

            g = Graphics.FromImage(bitmap);

            g.Clear(Color.White);

            drawLine(blackPenBold, 0, BOTTOM_OFFSET, PICTURE_WIDTH, BOTTOM_OFFSET);
            drawLine(blackPenBold, PICTURE_WIDTH / 2 - 50, 200 + BOTTOM_OFFSET, PICTURE_WIDTH / 2 + 50, 200 + BOTTOM_OFFSET);

            drawLine(blackPenBold, 0, 10, PICTURE_WIDTH / 2 - 50, 200 + BOTTOM_OFFSET);
            drawLine(blackPenBold, PICTURE_WIDTH / 2 + 50, 200 + BOTTOM_OFFSET, PICTURE_WIDTH, 10);

            drawLine(blackPenBold, 0.2f, BOTTOM_OFFSET, 0.2f, 10);
            drawLine(blackPenBold, PICTURE_WIDTH - 0.3f, BOTTOM_OFFSET, PICTURE_WIDTH - 0.4f, 10);

            for (int i = 0; i < 15; i++)
            {
                Note n = tonesDef[allToneList[i]];

                if (DRAW_TONE_LINES)
                {
                    drawLine(blackPen, 0, n.y_pos, PICTURE_WIDTH, n.y_pos);
                }
                else
                {
                    drawLine(blackPen, n.x_pos + 4, n.y_pos, n.x_pos + 13, n.y_pos);
                    drawLine(blackPen, PICTURE_WIDTH - n.x_pos - 4, n.y_pos, PICTURE_WIDTH - n.x_pos - 13, n.y_pos);
                }

                drawString(n.name, toneFont, blackBrush, n.x_pos + 3, n.y_pos - 1);
                drawString(n.name, toneFont, blackBrush, PICTURE_WIDTH - n.x_pos - 10, n.y_pos - 1);
            }
        }

        private void drawNote(Note note, float x_pos)
        {
            //log("drawTon [" + note.ton + ", " + x_pos + "]");

            if (note.long_ton == true)
            {
                fillEllipse(whiteBrush, x_pos - (TONE_WIDTH / 2), (float)(note.y_pos + (TONE_HEIGHT / 2)), TONE_WIDTH, TONE_HEIGHT);
                drawEllipse(redPenBold, x_pos - (TONE_WIDTH / 2), (float)(note.y_pos + (TONE_HEIGHT / 2)), TONE_WIDTH, TONE_HEIGHT);
            }
            else
            {
                fillEllipse(redBrush, x_pos - (TONE_WIDTH / 2), (float)(note.y_pos + (TONE_HEIGHT / 2)), TONE_WIDTH, TONE_HEIGHT);
            }
        }

        private void fillEllipse(Brush brush, float x, float y, float width, float height)
        {
            //log("drawEllipse [" + x + ", " + y + ", " + width + ", " + height + "]");

            g.FillEllipse(brush, x * PICTURE_WIDTH_MM,
                PICTURE_HEIGHT_PX - (y * PICTURE_HEIGHT_MM),
                width * PICTURE_WIDTH_MM,
                height * PICTURE_HEIGHT_MM);
        }

        private void drawEllipse(Pen pen, float x, float y, float width, float height)
        {
            //log("drawEllipse [" + x + ", " + y + ", " + width + ", " + height + "]");

            g.DrawEllipse(pen, x * PICTURE_WIDTH_MM,
                PICTURE_HEIGHT_PX - (y * PICTURE_HEIGHT_MM),
                width * PICTURE_WIDTH_MM,
                height * PICTURE_HEIGHT_MM);
        }

        private void drawLine(Pen p, float x, float y, float X, float Y)
        {
            //log("drawLine [" + x + ", " + y + ", " + X + ", " + Y + "]");

            g.DrawLine(p, x * PICTURE_WIDTH_MM,
                PICTURE_HEIGHT_PX - (y * PICTURE_HEIGHT_MM),
                X * PICTURE_WIDTH_MM,
                PICTURE_HEIGHT_PX - (Y * PICTURE_HEIGHT_MM));
        }

        private void drawString(String text, Font font, Brush brush, float x, float y)
        {
            //log("drawString [" + text + ", " + x + ", " + y + "]");

            if (text.Length > 2 && x > PICTURE_WIDTH / 2)
            {
                x -= 2;
            }

            g.DrawString(text, font,
                brush,
                x * PICTURE_WIDTH_MM,
                PICTURE_HEIGHT_PX - (y * PICTURE_HEIGHT_MM));
        }
    }
}