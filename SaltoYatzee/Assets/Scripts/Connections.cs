using System.Collections.Generic;

namespace Scripts
{
    public class Connections<T1,T2>
    {
        private readonly Dictionary<T1, T2> _fun = new Dictionary<T1, T2>();
        private readonly Dictionary<T2, T1> _inv = new Dictionary<T2, T1>();
        
        public void Connect(T1 t1, T2 t2)
        {
            if (_fun.ContainsKey(t1))
            {
                _fun[t1] = t2;
            }
            else
            {
                _fun.Add(t1, t2);
            }
            
            if (_inv.ContainsKey(t2)) {
                _inv[t2] = t1;
            }
            else
            {
                _inv.Add(t2, t1);    
            }
        }

        public bool Has(T1 t1)
        {
            return _fun.ContainsKey(t1);
        }
        
        public bool Has(T2 t2)
        {
            return _inv.ContainsKey(t2);
        }

        public T2 Get(T1 t1)
        {
            if (_fun.ContainsKey(t1)) return _fun[t1];
            throw new KeyNotFoundException();
        }

        public T1 Get(T2 t2)
        {
            if (_inv.ContainsKey(t2)) return _inv[t2];
            throw new KeyNotFoundException();
        }

        public void Remove(T1 t1)
        {
            if (_fun.ContainsKey(t1))
            {
                var value = _fun[t1];

                if (_inv.ContainsKey(value)) _inv.Remove(value);
                _fun.Remove(t1);
            }
        }
        
        public void Remove(T2 t2)
        {
            if (_inv.ContainsKey(t2))
            {
                var value = _inv[t2];

                _fun.Remove(value);
                _inv.Remove(t2);
            }
        }
    }
}
