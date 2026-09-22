using Platformer.Mechanics;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.EditorTools
{
    /// <summary>
    /// Groups scene tokens so designers can manage them as a set.
    /// Candidates may extend this component as needed.
    /// </summary>
    public class TokenGroup : MonoBehaviour
    {
        public List<TokenInstance> tokens = new List<TokenInstance>();


    }
}
