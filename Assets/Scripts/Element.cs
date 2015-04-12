using UnityEngine;
using System.Collections;

public class Element : MonoBehaviour {
    
    public Symbol Correspoding_Symbol;
    public float current;

    public virtual void process() { 
        
    }

    public virtual void passCurrent(float current) {
        this.current = current;
    }

    public virtual float getResistance() {
        return 0;
    }

    public virtual bool connected() {
        if (Correspoding_Symbol.terminal1 != null) if (Correspoding_Symbol.terminal1.wireGrid == null) return false;
        if (Correspoding_Symbol.terminal2 != null) if (Correspoding_Symbol.terminal2.wireGrid == null) return false;
        if (Correspoding_Symbol.terminal3 != null) if (Correspoding_Symbol.terminal3.wireGrid == null) return false;
        return true;
    }

    void OnMouseDown() { 
        
    }

}
