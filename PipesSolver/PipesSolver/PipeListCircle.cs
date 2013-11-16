using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PipesSolver
{
    class PipeListCircle
    {
        private List<Pipe> _Pipes;
        private int _Pointer;
        
        public PipeListCircle()
        {
            _Pipes = new List<Pipe>();
            _Pointer = 0;
            
        }

        public void Add(Pipe parNewPipe)
        {
            _Pipes.Add(parNewPipe);
        }

        public Pipe Next()
        {
            Pipe resPipe = _Pipes[_Pointer];
            _Pointer++;
            
            if (_Pointer >= _Pipes.Count) 
                _Pointer = 0;

            return resPipe;
        }

    }
}
