///////////////////////////////////////////////////////////////////////////////
//
//  File:           RegionCollection.cs
// 
//  Facility:		The unit contains the RegionCollection class.
//
//  Abstract:       The collection class of the Region class objects.
//
//  Environment:    VC 7.1
//
//  Author:         DCN Ltd.
//
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections;
using System.Drawing;

namespace DCN.TicTacToe.UI.SkinControl
{
    /// <summary>
    /// The collection class of regions.
    /// </summary>
    public class RegionCollection : DictionaryBase
    {

        #region Constructors

        /// <summary>
        /// Class constructor.
        /// </summary>
        public RegionCollection()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Class indexer.
        /// </summary>
        public Region this[String key]
        {
            get
            {
                return ((Region)Dictionary[key]);
            }
            set
            {
                Dictionary[key] = value;
            }
        }

        /// <summary>
        /// the property returns the collection of all keys of the dictionary.
        /// </summary>
        public ICollection Keys
        {
            get
            {
                return (Dictionary.Keys);
            }
        }

        /// <summary>
        /// the property returns the collection of all the values of the dictionary.
        /// </summary>
        public ICollection Values
        {
            get
            {
                return (Dictionary.Values);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// the function adds the value to the dictionary.
        /// </summary>
        /// <param name="key">êëþ÷</param>
        /// <param name="value">çíà÷åíèå</param>
        public void Add(String key, Region value)
        {
            Dictionary.Add(key, value);
        }

        /// <summary>
        /// the function checks whether the dictionary contains the value with the specified key.
        /// </summary>
        /// <param name="key">the key that will be used in checking whether the value corresponds to it.</param>
        /// <returns>returns true if the dictionary contains the value with the specified key.</returns>
        public bool Contains(String key)
        {
            return (Dictionary.Contains(key));
        }

        /// <summary>
        /// The function deletes the value corresponding to the key from the dictionary.
        /// </summary>
        /// <param name="key">êëþ÷, êîòîðûé áóäåò óäàë¸í</param>
        public void Remove(String key)
        {
            Dictionary.Remove(key);
        }

        #endregion

    }
}