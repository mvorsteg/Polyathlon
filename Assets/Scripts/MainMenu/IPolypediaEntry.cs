using System.Collections.Generic;
using UnityEngine;
public interface IPolypediaEntry
{
    string DisplayName { get; }
    string Description { get; }
    Sprite Thumbnail { get; }
    IList<Sprite> Slides { get; }
}