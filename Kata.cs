// Preloaded for you:
// Dictionary<string, string> ELEMENTS
// e.g. ELEMENTS["H"] == "Hydrogen"
using static Preloaded.Elements;

using System;
using System.Linq;
using System.Collections.Generic;

namespace Kata
{
    
  public class ElementalWords
  {

    public static string[][] ElementalForms(string word)
    {

      string[][] RV = EWs(word);
      return RV;   
    }
    
    private class Element
    {
      
      public int Level { get; set; }
      public Guid PreviousLevelId { get; set; }
      public Guid Id { get; set; }
      public string ElementKey { get; set; }
      
      public Element(int Level, Guid PreviousLevelId, Guid Id, string ElementKey)
      {
        this.Level = Level;
        this.PreviousLevelId = PreviousLevelId;
        this.Id = Id;
        this.ElementKey = ElementKey;
      }

    }

    public static string[][] EWs(string word)
    {
             string[][] RV;
      
            if (!string.IsNullOrEmpty(word))
            {
                List<Element> Elements = new List<Element>();

                CheckForElement(word, new Guid("00000000-0000-0000-0000-000000000000"), 0, ref Elements);

                if (Elements != null && Elements.Count > 0)
                {
                    int MaxLevel = Elements.Select((S) => S.Level).Distinct().OrderByDescending((OBD) => OBD).First();  //.Order().Last(); // .OrderDescending().First();
                    int Level = MaxLevel;

                    List<Element> ELs;
                    List<Element> NLs;

                    List<string> Ks = new List<string>();

                    string X;
                    Dictionary<string, string> Words = new Dictionary<string, string>();

                    string Key;
                    string Value;

                    while (Level > -1)
                    {
                        ELs = Elements.FindAll((EL) => EL.Level == Level);

                        foreach (Element EL in ELs)
                        {
                            NLs = Elements.FindAll((NL) => NL.Level == Level + 1 && NL.PreviousLevelId == EL.Id);

                            if (NLs.Count == 0)
                            {
                                Key = EL.ElementKey;
                                Value = $"{ELEMENTS[Key]} ({EL.ElementKey})";

                                ProcessElements(Level, Elements, EL.PreviousLevelId, ref Key, ref Value);

                                X = string.Empty;

                                foreach (string K in Key.Split("|"))
                                {
                                    X = $"{X}{K}";
                                }

                                if (X.ToLowerInvariant() == word.ToLowerInvariant())
                                {
                                    Words.Add(Key, Value);
                                }
                            }
                        }

                        Level--;
                    }

                    if (Words.Count > 0)
                    {
                        RV = new string[Words.Count][];

                        int DC = 0;
                        foreach (string W in Words.Values)
                        {
                            RV[DC] = W.Split("|");
                            DC++;
                        }
                    }
                    else
                    {
                        RV = new string[0][];
                    }
                }
                else
                {
                    RV = new string[0][];
                }
            }
            else
            {
                RV = new string[0][];
            }

            return RV;
    }

       static void ProcessElements(int Level, List<Element> Elements, Guid PreviousLevelId, ref string Key, ref string Value)
       {
           Level--;
           List<Element> ELs = Elements.FindAll((PL) => PL.Level == Level && PL.Id == PreviousLevelId);

           foreach (Element EL in ELs)
           {
               Key = $"{EL.ElementKey}|{Key}";
               Value = $"{ELEMENTS[EL.ElementKey]} ({EL.ElementKey})|{Value}";

               ProcessElements(Level, Elements, EL.PreviousLevelId, ref Key, ref Value);
           }
       }

       static void CheckForElement(string word, Guid PreviousLevelId, int Level, ref List<Element> Elements)
       {
           KeyValuePair<string, string> KVP;
           string Symbol;
           string SubWord;
           //string ElementFullName;
           Element Element;
           Guid Id;

           Symbol = word.Substring(0, 1);
           SubWord = word.Substring(1);
         
           KVP = ELEMENTS.FirstOrDefault((EL) => EL.Key.ToUpperInvariant() == Symbol.ToUpperInvariant() );

           if (KVP.Key != null)
           {
               Id = Guid.NewGuid();
               Element = new Element(Level, PreviousLevelId, Id, KVP.Key);
               Elements.Add(Element);

               if (SubWord.Length > 0)
               {
                   CheckForElement(SubWord, Id, Level + 1, ref Elements);
               }
           }

           if (2 <= word.Length)
           {
               Symbol = word.Substring(0, 2);
               SubWord = word.Substring(2);

             KVP = ELEMENTS.FirstOrDefault((EL) => EL.Key.ToUpperInvariant() == Symbol.ToUpperInvariant());

               if (KVP.Key != null)
               {
                   Id = Guid.NewGuid();
                   Element = new Element(Level, PreviousLevelId, Id, KVP.Key);
                   Elements.Add(Element);

                   if (SubWord.Length > 0)
                   {
                       CheckForElement(SubWord, Id, Level + 1, ref Elements);
                   }
               }
           }

           if (3 <= word.Length)
           {
               Symbol = word.Substring(0, 3);
               SubWord = word.Substring(3);

             KVP = ELEMENTS.FirstOrDefault((EL) => EL.Key.ToUpperInvariant() == Symbol.ToUpperInvariant());

               if (KVP.Key != null)
               {
                   Id = Guid.NewGuid();
                   Element = new Element(Level, PreviousLevelId, Id, KVP.Key);
                   Elements.Add(Element);

                   if (SubWord.Length > 0)
                   {
                       CheckForElement(SubWord, Id, Level + 1, ref Elements);
                   }
               }
           }
       }
  }

}