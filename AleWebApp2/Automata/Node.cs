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
            string init = "qi" + _counter;
            string fin = "qf" + _counter;
            List<TransitionFunction> funcs;
            switch (Token.Type)
            {
                case TokenType.Epsion:
                case TokenType.Letter:

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
                    s = Children[0].ThomsonConstruct();
                    t = Children[1].ThomsonConstruct();

                    funcs = new List<TransitionFunction>()
                    {
                        new TransitionFunction(init, s.InitialState, Constants.Epsilon),
                        new TransitionFunction(init, t.InitialState, Constants.Epsilon),
                        new TransitionFunction(s.FinalStates.FirstOrDefault(), fin, Constants.Epsilon),
                        new TransitionFunction(t.FinalStates.FirstOrDefault(), fin, Constants.Epsilon)
                    };

                    result.Alphabet = s.Alphabet.Union(t.Alphabet).Distinct().ToList();
                    result.States.Add(init);
                    result.States.Add(fin);
                    result.States.AddRange(s.States.Union(t.States).ToList());
                    result.InitialState = init;
                    result.FinalStates = new List<string> { fin };
                    result.Transitions = s.Transitions.Union(t.Transitions).ToList();
                    result.Transitions.AddRange(funcs);

                    break;
                case TokenType.KleeneStar:

                    s = Children[0].ThomsonConstruct();

                    funcs = new List<TransitionFunction>()
                    {
                        new TransitionFunction(init, fin, Constants.Epsilon),
                        new TransitionFunction(init, s.InitialState, Constants.Epsilon),
                        new TransitionFunction(s.FinalStates.FirstOrDefault(), fin, Constants.Epsilon),
                        new TransitionFunction(s.FinalStates.FirstOrDefault(), s.InitialState, Constants.Epsilon)
                    };

                    result.Alphabet = s.Alphabet.ToList();
                    result.States = s.States.ToList();
                    result.States.Add(init);
                    result.States.Add(fin);
                    result.InitialState = init;
                    result.FinalStates = new List<string> { fin };
                    result.Transitions = s.Transitions.ToList();
                    result.Transitions.AddRange(funcs);
                    

                    break;
            }
            return result;
        }








    }
}
