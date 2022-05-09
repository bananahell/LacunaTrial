using System.Collections;

namespace LacunaTrial {

  /// <summary> Class containing the logic behind the solution of the jobs demanded by Lacuna. Basically the model of this project. </summary>
  class BiologyDepartment {

    /// <summary> Does the decode strand job by decoding a base 64 string into a nucleobases A T C G string. </summary>
    /// <param name="strandEncoded"> The strand in base 64 string format. </param>
    /// <returns> The string representing the strand received in a nucleobases A T C G string. </returns>
    public static string DecodeStrand(string strandEncoded) {

      string strand = "";
      byte[] strandBytes;
      BitArray bitArr;

      // First acquire a byte array to then turn it into a BitArray and reverse its endianness.
      strandBytes = Convert.FromBase64String(strandEncoded);
      bitArr = new BitArray(strandBytes);
      bitArr = ReverseBitArrayEndianness(bitArr);

      // Determines which nucleobase is present based on the bits in the BitArray:
      // 00 = A  |  01 = C  |  10 = G  |  11 = T
      for (int i = 0; i < bitArr.Length; i += 2) {
        if (bitArr[0 + i] == false && bitArr[1 + i] == false) {
          strand += "A";
        } else if (bitArr[0 + i] == false && bitArr[1 + i] == true) {
          strand += "C";
        } else if (bitArr[0 + i] == true && bitArr[1 + i] == false) {
          strand += "G";
        } else {
          strand += "T";
        }
      }

      return strand;

    }

    /// <summary> Does the encode strand job by encoding a nucleobases A T C G string into a base 64 string. </summary>
    /// <param name="strand"> The strand in nucleobases A T C G string format. </param>
    /// <returns> The string representing the strand received in a base 64 string. </returns>
    public static string EncodeStrand(string strand) {

      string strandEncoded = "";
      List<bool> bitList = new List<bool>();
      byte[] strandEncodedBytes;

      // First encodes the nucleobases as:
      // A = 00  |  C = 01  |  G = 10  |  T = 11
      for (int i = 0; i < strand.Length; i++) {
        if (strand[i] == 'A') {
          bitList.Add(false);
          bitList.Add(false);
        } else if (strand[i] == 'C') {
          bitList.Add(false);
          bitList.Add(true);
        } else if (strand[i] == 'G') {
          bitList.Add(true);
          bitList.Add(false);
        } else {
          bitList.Add(true);
          bitList.Add(true);
        }
      }

      // Turns the bit list acquired from the nucleobases into bytes then turns those bytes into base 64.
      strandEncodedBytes = GetByteArrayFromBoolList(bitList);
      strandEncoded = Convert.ToBase64String(strandEncodedBytes);

      return strandEncoded;

    }

    /// <summary> Does the check gene job by checking if the gene is activated in the DNA strand. A gene is activated in a DNA strand when more than 50% of it is present in order inside the strand. </summary>
    /// <param name="strandEncoded"> The strand in base 64 string format. </param>
    /// <param name="geneEncoded"> The gene in base 64 string format. </param>
    /// <returns> Whether if the gene is activated inside the DNA strand or not. </returns>
    public static bool CheckGene(string strandEncoded, string geneEncoded) {

      string strand;
      string gene;
      string genePiece;

      // First decode both the strand and the gene
      strand = DecodeStrand(strandEncoded);
      gene = DecodeStrand(geneEncoded);

      // Checks if the strand is the template DNA strand, and if not, get template
      if (strand.Substring(0, 3) != "CAT") {
        strand = strand.Replace('C', 'X');
        strand = strand.Replace('G', 'C');
        strand = strand.Replace('X', 'G');
        strand = strand.Replace('A', 'X');
        strand = strand.Replace('T', 'A');
        strand = strand.Replace('X', 'T');
      }

      // Checks if a little more than 50% of the gene is present throughout the whole strand, returning true if present.
      for (int i = 0; i < (gene.Length - 1) / 2; i++) {
        genePiece = gene.Substring(0 + i, (gene.Length / 2) + 1);
        if (strand.Contains(genePiece)) {
          return true;
        }
      }

      return false;

    }

    /// <summary> Turns a list of bools into a byte array. </summary>
    /// <param name="boolList"> The list of bools about to be turned into a byte array. </param>
    /// <returns> The byte array equivalent to the list of bools. </returns>
    private static byte[] GetByteArrayFromBoolList(List<bool> boolList) {

      byte auxByte = 0;
      List<byte> auxList = new List<byte>();

      // In groups of 8 bits (a byte), add the left most bit (simple sum) then shift it to the left (multiply by 2) until done with the byte group.
      for (int i = 0; i < boolList.Count; i += 8) {
        auxByte += (byte)(boolList[0 + i] ? 1 : 0);  // Bit sum,
        auxByte = (byte)(auxByte * 2);               // then shift left by multiplying by 2.
        auxByte += (byte)(boolList[1 + i] ? 1 : 0);
        auxByte = (byte)(auxByte * 2);
        auxByte += (byte)(boolList[2 + i] ? 1 : 0);
        auxByte = (byte)(auxByte * 2);
        auxByte += (byte)(boolList[3 + i] ? 1 : 0);
        auxByte = (byte)(auxByte * 2);
        auxByte += (byte)(boolList[4 + i] ? 1 : 0);
        auxByte = (byte)(auxByte * 2);
        auxByte += (byte)(boolList[5 + i] ? 1 : 0);
        auxByte = (byte)(auxByte * 2);
        auxByte += (byte)(boolList[6 + i] ? 1 : 0);
        auxByte = (byte)(auxByte * 2);
        auxByte += (byte)(boolList[7 + i] ? 1 : 0);

        // Add the newfound byte to the list of bytes to be returned, then restart process.
        auxList.Add(auxByte);
        auxByte = 0;
      }

      return auxList.ToArray();

    }

    /// <summary> Reverses the endianness of a BitArray by reversing the order of bits in each byte. </summary>
    /// <param name="bitArr"> The BitArray about to be manipulated. </param>
    /// <returns> The same BitArray received with all of its bytes reversed. </returns>
    private static BitArray ReverseBitArrayEndianness(BitArray bitArr) {

      Boolean auxBool0;
      Boolean auxBool1;
      Boolean auxBool2;
      Boolean auxBool3;

      // Checks if this BitArray is actually representing groups of bytes by seeing if its size is a multiple of 8 bits.
      if (bitArr.Length % 8 != 0) {
        Console.WriteLine("BitArray received to reverse endianness was not divisible by 8, but why?");
        throw new Exception();
      }

      // In groups of 8 bits (a byte), switch the right most bit with the left most, then the second right most with the second left most, and so on.
      for (int i = 0; i < bitArr.Length; i += 8) {
        // Some aux variables for this switch operation.
        auxBool0 = bitArr[0 + i];
        auxBool1 = bitArr[1 + i];
        auxBool2 = bitArr[2 + i];
        auxBool3 = bitArr[3 + i];
        // 0 <-> 7  |  1 <-> 6  |  2 <-> 5  |  3 <-> 4
        bitArr[0 + i] = bitArr[7 + i];
        bitArr[1 + i] = bitArr[6 + i];
        bitArr[2 + i] = bitArr[5 + i];
        bitArr[3 + i] = bitArr[4 + i];
        bitArr[4 + i] = auxBool3;
        bitArr[5 + i] = auxBool2;
        bitArr[6 + i] = auxBool1;
        bitArr[7 + i] = auxBool0;
      }

      return bitArr;

    }

  }

}
