
using System;
using System.Collections;

namespace DCN.TicTacToe.UI.SkinComponents
{
    /// <summary>
    /// The class presenting the collection of text elements that are arranged	
    /// on SkinTooltip.
    /// </summary>
    public class LabelCollection : CollectionBase
    {
        #region Properties

        /// <summary>
        /// The class indexer.
        /// </summary>
        public LabelItem this[int index]
        {
            get { return (LabelItem)List[index]; }
            set { List[index] = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The method adding elements to the collection.
        /// </summary>
        /// <param name="item">Ýëåìåíò äëÿ äîáàâëåíèÿ.</param>
        /// <returns>returnes the index of the added element.</returns>
        public int Add(LabelItem item)
        {
            return List.Add(item);
        }

        /// <summary>
        /// The method deleting elements from the collection.
        /// </summary>
        /// <param name="item"> The element to be deleted. </param>
        public void Remove(LabelItem item)
        {
            List.Remove(item);
        }

        #endregion
    }
}