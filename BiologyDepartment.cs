using System.Collections;

namespace LacunaTrial {

  class BiologyDepartment {

    public static string DecodeStrand(string strandEncoded) {

      string strand = "";
      byte[] strandBytes;
      BitArray bitArr;

      strandBytes = Convert.FromBase64String(strandEncoded);
      bitArr = new BitArray(strandBytes);
      bitArr = ReverseBitArrayEndianness(bitArr);
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

    public static string EncodeStrand(string strand) {

      string strandEncoded = "";
      List<bool> bitList = new List<bool>();
      byte[] strandEncodedBytes;

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
      strandEncodedBytes = GetByteArrayFromBoolList(bitList);
      strandEncoded = Convert.ToBase64String(strandEncodedBytes);

      return strandEncoded;

    }

    public static bool CheckGene(string strandEncoded, string geneEncoded) {

      string strand;
      string gene;
      string genePiece;

      strand = DecodeStrand(strandEncoded);
      gene = DecodeStrand(geneEncoded);

      for (int i = 0; i < (gene.Length - 1) / 2; i++) {
        genePiece = gene.Substring(0 + i, (gene.Length / 2) + 1);
        if (strand.Contains(genePiece)) {
          return true;
        }
      }

      return false;

    }

    private static byte[] GetByteArrayFromBoolList(List<bool> boolList) {

      byte auxByte = 0;
      List<byte> auxList = new List<byte>();

      for (int i = 0; i < boolList.Count; i += 8) {
        auxByte += (byte)(boolList[0 + i] ? 1 : 0);
        auxByte = (byte)(auxByte * 2);
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

        auxList.Add(auxByte);
        auxByte = 0;
      }

      return auxList.ToArray();

    }

    private static BitArray ReverseBitArrayEndianness(BitArray bitArr) {

      Boolean auxBool0;
      Boolean auxBool1;
      Boolean auxBool2;
      Boolean auxBool3;

      if (bitArr.Length % 8 != 0) {
        Console.WriteLine("BitArray received to reverse endianness was not divisible by 8, but why?");
        throw new Exception();
      }

      for (int i = 0; i < bitArr.Length; i += 8) {
        auxBool0 = bitArr[0 + i];
        auxBool1 = bitArr[1 + i];
        auxBool2 = bitArr[2 + i];
        auxBool3 = bitArr[3 + i];

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
