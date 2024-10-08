using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arithmetic
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Giriş metni, boşluklar kaldırılarak ve büyük harfler küçük harfe dönüştürülerek hazırlanır.
            string input = "baba caddede gec".Replace(" ", "").ToLower(); // Boşlukları kaldır ve küçük harfe çevir
            Dictionary<char, int> frequency = new Dictionary<char, int>(); // her karakterin freknasını saklamak için sözlük oluşturuyoruz.

            // Frekansları hesapla
            foreach (char c in input) 
            {
                if (frequency.ContainsKey(c)) // eğer karakter varsa bir arttır yoksa frekansı bir olarak başlatır.
                    frequency[c]++;
                else
                    frequency[c] = 1;
            }

            // Toplam sembol sayısı hesaplanır
            int totalSymbols = input.Length;

            // Olasılık aralıklarını hesaplamak için sözlük
            Dictionary<char, (double low, double high)> probabilityRanges = new Dictionary<char, (double low, double high)>(); // her karakter için low ve high değerlerini tutar 
            double cumulative = 0.0; // aralık değerini tutar

            // Alfabetik sıraya göre frekans tablosunu işle
            foreach (var kvp in frequency.OrderBy(x => x.Key)) // Alfabetik sıralama
            {
                double probability = (double)kvp.Value / totalSymbols; //frekans değerini toplam karaktere bölerek olasılığı bulur 
                probabilityRanges[kvp.Key] = (cumulative, cumulative + probability); // başlangıç ve bitiş aralığı hesaplanır 
                cumulative += probability; // bir sonraki karakterin hesaplanması için güncellenir.
            }

            // Olasılık aralıklarını yazdır
            Console.WriteLine("Olasılık Aralıkları:");
            foreach (var kvp in probabilityRanges)
            {
                Console.WriteLine($"'{kvp.Key}': [{kvp.Value.low}, {kvp.Value.high}), Olasılık: {frequency[kvp.Key]} / {totalSymbols} = {kvp.Value.high - kvp.Value.low}");
            }

            // Aritmetik kodlama (formül)
            double low = 0.0; //en düşük ve en yüksek aralıklar belirlenir 
            double high = 1.0;

            foreach (char c in input)
            {
                var range = probabilityRanges[c]; // olasılık aralığı alınır 
                double rangeWidth = high - low; // aralık genişiliği hesaplanır 

                high = low + rangeWidth * range.high; // aralığın yüksek değeri için mevcut aralık değeri ile karakterin yüksek aralığı çarpılıp başlangıç değerine eklenir.
                low = low + rangeWidth * range.low;//
            }

            // Nihai kodu hesapla
            double finalCode = (low + high) / 2; 

            // Bit cinsinden boyutu hesapla
            double bits = -Math.Log(finalCode, 2);

            // Sonuçları yazdır
            Console.WriteLine($"\nSıkıştırılmış değer: {finalCode}");
            Console.WriteLine($"Bit boyutu: {bits}");
        }
    }
}
