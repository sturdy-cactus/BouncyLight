using System.Diagnostics;
using System.IO;
using System.Text;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace PFMlib;
public struct Color
{
	//MEMBERS
	public float r, g, b;

	//COSTRUTTORE DEFAULT
	public Color()
	{
		r = 0f;
		g = 0f;
		b = 0f;
	}

	//COSTRUTTORE 
	public Color(float r, float g, float b)
    {
		this.r = r;
		this.g = g;
		this.b = b;
    }

	//METODI
	public static Color Sum(Color x, Color y)
    {
		var c = new Color(x.r + y.r, x.g + y.b, x.b + y.b);
		return c;
    }

	public static Color Mult(Color x, float a)
    {
		var c = new Color(a*x.r, a*x.g, a*x.b);
		return c;
	}

	public static Color Mult(Color x, Color y)
    {
		var c = new Color(x.r * y.r, x.g * y.b, x.b * y.b);
		return c;
	}

	public float Luminosity()
    {	//correzione_bug: c'era un segno +!
		return (Math.Max(this.r, Math.Max(this.g, this.b)) - Math.Min(this.r, Math.Min(this.g, this.b))) / 2f;
    }
	
	//OPERATORS
	public static Color operator *(float a, Color c)
	{
		var color = new Color(a*c.r, a*c.g, a*c.b);
		return color;
	}

	public static Color operator +(Color a, Color b)
	{
		return new Color(a.r + b.r, a.g + b.g, a.b + b.b);
	}

	public static Color operator *(Color a, Color b)
	{
		return new Color(a.r * b.r, a.g * b.g, a.b * b.b);
	}
	
}

public class HdrImage 	
{
	//MEMBERS
	public int w;
	public int h;

	public Color[] pixels;

	//COSTRUTTORE BASE
	public HdrImage(int w, int h)
    {
		this.w = w;
		this.h = h;
		pixels = new Color[w*h];
    }

	//COSTRUTTORE DA PATH
	public HdrImage(string path)
    {
		//apertura stream e inizializzazione
		int offset = 0;
		FileStream fs = null;
		StreamReader sr = null;
		try
		{
			fs = new FileStream(path, FileMode.Open);
			sr = new StreamReader(fs);
		}
		catch(Exception)
		{
			Console.WriteLine("Impossibile aprire il file!");
			Environment.Exit(2);
		}

		//riga 1
		string magic = sr.ReadLine();
		offset += magic.Length;
		if (magic != "PF")
		{
			Console.WriteLine("il file non e' PFM");
			Environment.Exit(3);
		}

		//riga2
		string dim = sr.ReadLine();
		offset += dim.Length;

		//riga3
		bool endianness = false;//true se little endian
		try
		{
			string endianness_string = sr.ReadLine();
			offset += endianness_string.Length;

			if (Single.Parse(endianness_string) < 0)
				endianness = true;
		}
		catch (Exception)
		{
			Console.WriteLine("Error while reading endianness");
			Environment.Exit(4);
		}

		//istanziazione membri
		try
		{
			w = HdrImage.StringToDim(dim)[0];
			h = HdrImage.StringToDim(dim)[1];
		}
		catch (Exception)
		{
			Console.WriteLine("Errore nella lettura della risoluzione");
			Environment.Exit(5);
		}

		pixels = new Color[h * w];

		try
		{
			//lettura bytes
			HdrImage.MyByteReader(w, h, endianness, offset, fs, ref pixels);
		}
		catch (Exception)
		{
			Console.WriteLine("Errore nella lettura dei pixel!");
			Environment.Exit(6);
		}

		Array.Reverse(pixels);
		HdrImage.FlipImg(w, h, ref pixels);

		//chiusura 
		sr.Close();
		fs.Close();
    }

	//METODI
	public Color GetPixel(int row, int col)
    {
		return pixels[w*row + col];
    }
	
	public void SetPixel(Color val, int row, int col)
    {
		pixels[w * row + col] = val;
	}

	public void SaveHdrImage(string path)
    {
		//prep
		float endianness_value = 1.0f;
		if (BitConverter.IsLittleEndian)
			endianness_value = -1.0f;
		HdrImage.FlipImg(w, h, ref pixels);
		Array.Reverse(pixels);
		var header = Encoding.UTF8.GetBytes($"PF\n{w} {h}\n{endianness_value}\n");
		var fs = new FileStream(path, FileMode.CreateNew, FileAccess.Write);
		
		//scrittura
		fs.Write(header, 0, header.Length);
        for (int i = 0; i < w * h; i++)
        {
			HdrImage.WriteFloat(fs, pixels[i].r);
			HdrImage.WriteFloat(fs, pixels[i].g);
			HdrImage.WriteFloat(fs, pixels[i].b);
		}

		//chiusura
		fs.Close();
		//ripristino originale
		Array.Reverse(pixels);
		HdrImage.FlipImg(w, h, ref pixels);
    }

	public float Avg_Lum()
	{
		float delta = 1e-10f;
		float log_avg = 0;
		foreach (var pixel in pixels) 
			log_avg += (float)(Math.Log10(delta + pixel.Luminosity())/pixels.Length);
		
		return (float)Math.Pow(10,log_avg);
	}

	public void NormalizeImg(float a, float l)
    {
		for (int i = 0; i < this.pixels.Length; i++)
        {
			this.pixels[i].r = a* this.pixels[i].r / l;
			this.pixels[i].g = a * this.pixels[i].g / l;
			this.pixels[i].b = a * this.pixels[i].b / l;
		}
    }

	public void ClampImg()
    {
		for (int i = 0; i < this.pixels.Length; i++)
		{
			this.pixels[i].r = HdrImage.Clamp(this.pixels[i].r);
			this.pixels[i].g = HdrImage.Clamp(this.pixels[i].g);
			this.pixels[i].b = HdrImage.Clamp(this.pixels[i].b);
		}
	}

	public HdrImage TosRGB(float? factor=null, float? luminosity = null, float? gamma=null)
	{
		HdrImage Normalized = this;
		var a = factor ?? 0.2f;
		var lum = luminosity ?? Avg_Lum();
		var gam = gamma ?? 1f;
		Color c=new Color();
		for (int i = 0; i < pixels.Length; i++)
		{
			pixels[i].r = (int) (255 * Math.Pow(Clamp(pixels[i].r * (a / lum)), 1 / gam));
			pixels[i].g = (int) (255 * Math.Pow(Clamp(pixels[i].g * (a / lum)), 1 / gam));
			pixels[i].b = (int) (255 * Math.Pow(Clamp(pixels[i].b * (a / lum)), 1 / gam));
		}

		return Normalized;
	}

	public void SaveLdrImg(string path, float? lum=null)
	{
		var luminosity = lum ?? Avg_Lum();
		HdrImage converted = this.TosRGB(luminosity:luminosity);
		//creation di Bitmap
		Bitmap bmp = new Bitmap(this.w, this.h);
		for (int i = 0; i < this.w; i++)
		{
			for (int j = 0; j < this.h; j++)
			{
				System.Drawing.Color color = System.Drawing.Color.FromArgb((int) converted.GetPixel(j, i).r,
					(int) converted.GetPixel(j, i).g, (int) converted.GetPixel(j, i).b);
				bmp.SetPixel(i, j, color);
			}
		}
		//obtain vero nome file e type
		string nome = path.Split('\\').Last();
		string type = path.Split('.').Last();

		try
		{
			//salvataggio
			if (type is "png" or "PNG")
				bmp.Save(nome, ImageFormat.Png);
			if (type is "jpg" or "JPG" or "jpeg" or "JPEG")
				bmp.Save(nome, ImageFormat.Jpeg);
			else throw new Exception("Invalid file format");
		}
		catch (Exception)
		{
			Console.WriteLine("Errore nel salvataggio del file. I formati validi sono png o jpeg.");
			Environment.Exit(8);
		}
	}

	//METODI PRIVATI
	private static int[] StringToDim(string stringa)
    {
		var array = new int[2];
		int j = 0;
		while (stringa[j] != ' ')
			j++;

		var heightchars = new char[j + 1];
		for (int i = 0; i < heightchars.Length; i++)
			heightchars[i] = stringa[i];

		var widthchars = new char[stringa.Length - j];
		for (int i = 0; i < widthchars.Length; i++)
			widthchars[i] = stringa[j + i];

		string heightstring = new string(heightchars);
		string widthstring = new string(widthchars);

		array[0] = Int32.Parse(heightstring);
		array[1] = Int32.Parse(widthstring);

		return array;
	}
	private static void MyByteReader(int width, int height, bool end, int off, FileStream stream, ref Color[] pix)
	{
		stream.Seek(off + 3, SeekOrigin.Begin);
		byte[] tone = new byte[4];

		for (int i = 0; i < width * height; i++)
		{
			for (int j = 0; j < 4; j++)
				tone[j] = Convert.ToByte(stream.ReadByte());
			if (end != BitConverter.IsLittleEndian)
				Array.Reverse(tone);
			pix[i].r = BitConverter.ToSingle(tone, 0); 

			for (int j = 0; j < 4; j++)
				tone[j] = Convert.ToByte(stream.ReadByte());
			if (end != BitConverter.IsLittleEndian)
				Array.Reverse(tone);
			pix[i].g = BitConverter.ToSingle(tone, 0);

			for (int j = 0; j < 4; j++)
				tone[j] = Convert.ToByte(stream.ReadByte());
			if (end != BitConverter.IsLittleEndian)
				Array.Reverse(tone);
			pix[i].b = BitConverter.ToSingle(tone, 0);
		}
	}
	private static void FlipImg(int width, int height, ref Color[] pix)
    {
		var pix_copy = new Color[pix.Length];
		for (int i = 0; i < pix.Length; i++)
        {
			pix_copy[i].r = pix[i].r;
			pix_copy[i].g = pix[i].g;
			pix_copy[i].b = pix[i].b;
        }
		
		for (int j = 0; j < height; j++)
		{
			for (int i = 0; i < width; i++)
			{
				pix[i + j * width].r = pix_copy[width*(j+1) - i - 1].r;
				pix[i + j * width].g = pix_copy[width*(j+1) - i - 1].g;
				pix[i + j * width].b = pix_copy[width*(j+1) - i - 1].b;
			}
		}
	}
	private static void WriteFloat(FileStream output, float val)
	{
		byte[] seq = BitConverter.GetBytes(val);
		output.Write(seq, 0, seq.Length);
	}
	private static float Clamp(float a)
    {
		return a / (a + 1f);
	}
}
