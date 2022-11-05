using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTG_Tools.ClassesStructs.Text
{
    public struct Landb
    {
        public uint stringNumber; //Current string number
        public uint anmID; //Actor's animation
        public uint wavID; //Actor's speech
        public int zero1; //Some zero value

        //anm and wav file names
        public int blockAnmNameSize;
        public int anmNameSize;
        public string anmName;
        public int blockWavNameSize;
        public int wavNameSize;
        public string wavName;

        public int blockUnknownNameSize; //I don't know what does it mean
        public int unknownNameSize;
        public string unknownName;

        public int zero2; //Another some zero value

        public int blockLangresSize;
        
        public int blockActorNameSize;
        public int actorNameSize;
        public string actorName;

        public int blockActorSpeechSize;
        public int actorSpeechSize;
        public string actorSpeech;

        public int blockSize;
        public int someValue;

        public int flags;
    }

    /*public struct SomeDateAfterLandb
    {
        public int commonBlockSize;

        public int firstBlockSize;
        public int firstCountElements;
        public int[] firstNameSize;
        public string[] firstName;

        public int secondBlockSize;
        public int secondCountElements;
        public int[] secondNameSize;
        public string[] secondName;
    }

    public struct LastLandbData //16 bytes of some data
    {
        public int Unknown1;
        public int Unknown2;
        public int Unknown3;
        public int Unknown4;
    }*/

    public class LandbClass
    {
        //public bool isBlockLength; Maybe I'll remove that
        public int blockSize1;
        public int someValue1;
        public int blockSize2;
        public int someValue2;

        public int blockLength;
        public int landbCount;

        public Landb[] landbs;
        public FlagsClass.LangdbFlagClass[] flags;
        public byte[] unknownData;
        /*public SomeDateAfterLandb someAfterData;
        public LastLandbData lastLandbData;*/
    }
}
