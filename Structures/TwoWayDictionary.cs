// Duck typing. Yeah.
using System.Collections.Generic;

namespace Aspirations {
  public class TwoWayDictionary<Me, You> {
    Dictionary<Me, You> MeToYou;
    Dictionary<You, Me> YouToMe;

    public int Count {
      get { return MeToYou.Count; }
    }

    public TwoWayDictionary() {
      MeToYou = new Dictionary<Me, You>();
      YouToMe = new Dictionary<You, Me>();
    }

    public TwoWayDictionary(int length) {
      MeToYou = new Dictionary<Me, You>(length);
      YouToMe = new Dictionary<You, Me>(length);
    }

    public You this[Me index] {
      get { return MeToYou[index]; }
      set {
        MeToYou[index] = value;
        YouToMe[value] = index;
      }
    }

    public Me this[You index] {
      get { return YouToMe[index]; }
      set {
        YouToMe[index] = value;
        MeToYou[value] = index;
      }
    }

    public bool ContainsKey(You id) {
      return YouToMe.ContainsKey(id);
    }

    public bool ContainsKey(Me id) {
      return MeToYou.ContainsKey(id);
    }

    public bool Remove(You id) {
      if (!ContainsKey(id)) return false;
      Me me = YouToMe[id];
      MeToYou.Remove(me);
      YouToMe.Remove(id);
      return true;
    }

    public bool Remove(Me id) {
      if (!ContainsKey(id)) return false;
      You me = MeToYou[id];
      YouToMe.Remove(me);
      MeToYou.Remove(id);
      return true;
    }

    public List<You> Values {
      get { return new List<You>(YouToMe.Keys); }
    }

    public List<Me> Keys {
      get { return new List<Me>(MeToYou.Keys); }
    }
  }
}