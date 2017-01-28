using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Maca;

public class Motor : MonoBehaviour {

    public int[] puzzleGrid;
    public int[] questionsTroughRight;
    public int[] questionsTroughDown;

    public List<string> questions;
    public List<int> imageNumber;

    //public GameObject image1;
    //public GameObject image2;
    //public GameObject image3;
    //public GameObject image4;
    //public GameObject image5;
    //public GameObject image6;
    //public GameObject image7;
    //public GameObject image8;

    public void Awake()
    {
        if(GUIManager.isPseudo)
        {
            pseudoPuzzle();
        }

        else
        {
            createPuzzle();
        }
    }
    void Start () {
		
	}
	
	void Update () {
		
	}

    void pseudoPuzzle()
    {
        puzzleGrid = new int[] 
        {

        0, 0, 0, 0, 1, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 1, 1, 0, 1, 0,
        0, 0, 0, 0, 1, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        1, 0, 0, 0, 0, 1, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 1, 1, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 1, 1, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 1, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 1, 1, 0, 1, 0,
        0, 0, 0, 0, 1, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        1, 0, 0, 0, 0, 1, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 1, 1, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 1, 1, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 1, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 1, 1, 0, 1, 0,
        0, 0, 0, 0, 1, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        1, 0, 0, 0, 0, 1, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 1, 1, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 1, 1, 0, 0, 0, 0, 0, 0, 0,

        };

        questionsTroughRight = new int[] 
        {

        1,  1,  1,  1,  0,  2,  2,  2,  2,  2,
        3,  3,  3,  3,  3,  0,  0,  0,  0,  0,
        4,  4,  4,  4,  0,  5,  5,  5,  5,  5,
        6,  6,  6,  6,  6,  6,  6,  6,  6,  0,
        7,  7,  7,  7,  7,  7,  7,  7,  7,  7,
        8,  8,  8,  8,  8,  8,  8,  8,  8,  8,
        0,  9,  9,  9,  9,  0,  10, 10, 10, 10,
        11, 11, 11, 11, 11, 0,  0,  12, 12, 12,
        13, 13, 13, 13, 13, 13, 13, 13, 13, 13,
        0,  0,  0,  14, 14, 14, 14, 14, 14, 14,

        };

        questionsTroughDown = new int[] 
        {

        15, 17, 18, 19, 0,  0,  0,  25, 0,  27,
        15, 17, 18, 19, 0,  0,  0,  25, 0,  27,
        15, 17, 18, 19, 0,  21, 23, 25, 26, 27,
        15, 17, 18, 19, 20, 21, 23, 25, 26, 0,
        15, 17, 18, 19, 20, 21, 23, 25, 26, 27,
        15, 17, 18, 19, 20, 21, 23, 25, 26, 27,
        0,  17, 18, 19, 20, 0,  23, 25, 26, 27,
        16, 17, 18, 19, 20, 0,  0,  25, 26, 27,
        16, 17, 18, 19, 20, 22, 24, 25, 26, 27,
        16, 0,  0,  19, 20, 22, 24, 25, 26, 27,

        };

        /*00*/ questions.Add("");
        /*01*/ questions.Add("Resimdeki aktris");
        /*02*/ questions.Add("Açık artırım ile satış");
        /*03*/ questions.Add("Baş harfler ile atılan imza");
        /*04*/ questions.Add("Resimdeki Oscar ödüllü film");
        /*05*/ questions.Add("Afrika’da yaşayan bir tür antilop");
        /*06*/ questions.Add("Resimdeki film");
        /*07*/ questions.Add("Aynı çizgi üzerinde olma durumu");
        /*08*/ questions.Add("Divit yazı hokkası");
        /*09*/ questions.Add("Bir ilimiz");
        /*10*/ questions.Add("Cem Yılmaz'ın bir filmi");
        /*11*/ questions.Add("Resimdeki film");
        /*12*/ questions.Add("Arap Yarımadası'nda bir körfez");
        /*13*/ questions.Add("İsviçre'de bir nehir");
        /*14*/ questions.Add("Dişi deve");
        /*15*/ questions.Add("Resimdeki gezegen");
        /*16*/ questions.Add("Türk Müziği'nde bir makam");
        /*17*/ questions.Add("Gemi omurgası");
        /*18*/ questions.Add("Resimdeki film");
        /*19*/ questions.Add("Parmak ucuyla tutulabilen miktar");
        /*20*/ questions.Add("Minyatür ağaç yetiştirme sanatı");
        /*21*/ questions.Add("Trabzonspor'un şampiyonluk sayısı");
        /*22*/ questions.Add("Antik zaman testisi");
        /*23*/ questions.Add("Öküz yemliği");
        /*24*/ questions.Add("İri taneli bezelye");
        /*25*/ questions.Add("Resimdeki aktör");
        /*26*/ questions.Add("Altüst etme");
        /*27*/ questions.Add("İşkence bölmesi");
        /*28*/ questions.Add("Türkiye Büyük Millet Meclisi");
        /*29*/ questions.Add("Satrançta bir hareket");

        /*00*/ imageNumber.Add(0);
        /*01*/ imageNumber.Add(1);
        /*02*/ imageNumber.Add(0);
        /*03*/ imageNumber.Add(0);
        /*04*/ imageNumber.Add(2);
        /*05*/ imageNumber.Add(0);
        /*06*/ imageNumber.Add(3);
        /*07*/ imageNumber.Add(0);
        /*08*/ imageNumber.Add(0);
        /*09*/ imageNumber.Add(0);
        /*10*/ imageNumber.Add(0);
        /*11*/ imageNumber.Add(4);
        /*12*/ imageNumber.Add(0);
        /*13*/ imageNumber.Add(0);
        /*14*/ imageNumber.Add(0);
        /*15*/ imageNumber.Add(5);
        /*16*/ imageNumber.Add(0);
        /*17*/ imageNumber.Add(0);
        /*18*/ imageNumber.Add(6);
        /*19*/ imageNumber.Add(0);
        /*20*/ imageNumber.Add(7);
        /*21*/ imageNumber.Add(0);
        /*22*/ imageNumber.Add(0);
        /*23*/ imageNumber.Add(0);
        /*24*/ imageNumber.Add(0);
        /*25*/ imageNumber.Add(8);
        /*26*/ imageNumber.Add(0);
        /*27*/ imageNumber.Add(0);
        /*28*/ imageNumber.Add(0);
        /*29*/ imageNumber.Add(0);
    }

    void createPuzzle()
    {
        
    }
}
