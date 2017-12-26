using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Automata
{
    public class Node
    {
        private readonly List<Node> _children =
            new List<Node>();

        public readonly Guid Id;
        public Token Token { get; }
        public Node Parent { get; private set; }

        public Node(Guid id, Token token)
        {
            this.Id = id;
            Token = token;
        }



        public void Add(Node item)
        {
            if (item.Parent != null)
            {
                item.Parent._children.Remove(item);
            }

            item.Parent = this;
            this._children.Add(item);
        }

        public List<Node> Children => _children;

        static int _counter = 0;
        public FiniteStateAutomaton ThomsonConstruct()
        {
            _counter = _counter + 1;
            FiniteStateAutomaton result = new FiniteStateAutomaton();
            FiniteStateAutomaton s;
            FiniteStateAutomaton t;
            switch (Token.Type)
            {
                case TokenType.Epsion:
                case TokenType.Letter:
                    var init = "q" + _counter;
                    var fin = "q" + _counter;

                    result.Alphabet = Token.IsLetter ? new List<char> { Token.Char } : new List<char>();
                    result.States = new List<string> { init, fin };
                    result.InitialState = init;
                    result.FinalStates = new List<string> { fin };
                    result.Transitions = new List<TransitionFunction> { new TransitionFunction(init, fin, Token.Char) };

                    break;
                case TokenType.Concatenation:
                    s = Children[0].ThomsonConstruct();                    
                    t = Children[1].ThomsonConstruct();
                    List<TransitionFunction> sFuncs = s.Transitions.Where(x => x.EndState == s.FinalStates.FirstOrDefault()).ToList();
                    foreach (TransitionFunction func in sFuncs)
                    {
                        func.EndState = t.InitialState;
                    }
                    foreach (var sFinalState in s.FinalStates)
                    {
                        s.States.Remove(sFinalState);
                    }
                    result.Alphabet = s.Alphabet.Union(t.Alphabet).Distinct().ToList();
                    result.States = s.States.Union(t.States).ToList();
                    result.Transitions = s.Transitions.Union(t.Transitions).ToList();
                    
                    result.InitialState = s.InitialState;
                    result.FinalStates = t.FinalStates;
                    
                    break;
                case TokenType.Union:
                    var initUn = "q" + _counter;
                    var finUn = "q" + _counter;
                    s = Children[0].ThomsonConstruct();
                    t = Children[1].ThomsonConstruct();

                    TransitionFunction tOr1 = new TransitionFunction(initUn,s.InitialState,Constants.Epsilon);
                    TransitionFunction tOr2 = new TransitionFunction(initUn, t.InitialState, Constants.Epsilon);
                    TransitionFunction tOr3 = new TransitionFunction(s.FinalStates.FirstOrDefault(),finUn,Constants.Epsilon);
                    TransitionFunction tOr4 = new TransitionFunction(t.FinalStates.FirstOrDefault(), finUn, Constants.Epsilon);

                    result.Alphabet = s.Alphabet.Union(t.Alphabet).Distinct().ToList();
                    result.States.Add(initUn);
                    result.States.Add(finUn);
                    result.States.AddRange(s.States.Union(t.States).ToList());
                    result.Transitions = s.Transitions.Union(t.Transitions).ToList();
                    result.Transitions.Add(tOr1);
                    result.Transitions.Add(tOr2);
                    result.Transitions.Add(tOr3);
                    result.Transitions.Add(tOr4);
                    result.InitialState = initUn;
                    result.FinalStates = new List<string>{finUn};
                    


                    break;
                case TokenType.KleeneStar:


                    break;
            }
            //result.Alphabet = s.Alphabet.Union(t.Alphabet).ToList();
            return result;
        }








    }
}
