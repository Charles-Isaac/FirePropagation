using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace FirePropagation
{

    public enum TypeOfTile
    {
        Tree = 0,
        Fire = 1,
        Burnt = 2,
    }

    public abstract class Tile
    {
        protected int ChangePercent = 30;
        protected int m_TypeOfTile;
        protected double m_LastEventTime;
        protected double m_NextEvenTime;
        protected double m_Cooldown;
        protected Tile[,] m_ArrayOfItself;
        protected Point m_Position;
        protected Random m_RNG;
        protected double m_CurrentTime;

        public Tile(TypeOfTile type)
        {
            m_TypeOfTile = (int) type;
        }

        public int TypeTile
        {
            get { return m_TypeOfTile; }
            set { m_TypeOfTile = value; }
        }

        public double LastEventTime
        {
            get { return m_LastEventTime; }
            set { m_LastEventTime = value; }
        }

        public double Cooldown
        {
            get { return m_Cooldown; }
            set { m_Cooldown = value; }
        }

        public Tile[,] ArrayOfItself
        {
            get { return m_ArrayOfItself; }
            set { m_ArrayOfItself = value; }
        }

        public Point Position
        {
            get { return m_Position; }
            set { m_Position = value; }
        }

        public Random RNG
        {
            get { return m_RNG; }
            set { m_RNG = value; }
        }

        public double CurrentTime
        {
            get { return m_CurrentTime; }
            set { m_CurrentTime = value; }
        }

        public bool ShouldUpdate(double CurrentTime)
        {
            return CurrentTime > m_NextEvenTime;
        }

        public abstract void Update(double CurrentTime);
        public abstract Tile ToPrimitive();
    }

    public class TreeTile : Tile
    {
        public TreeTile(double _CurrentTime, Tile[,] ArrayOfItself, Point Position, Random RNG) : base(TypeOfTile.Tree)
        {
            ChangePercent = 50;
            LastEventTime = _CurrentTime;
            m_CurrentTime = _CurrentTime;
            Cooldown = 15000 + RNG.Next(5000); ;//15sec
            m_NextEvenTime = _CurrentTime + Cooldown;
            base.ArrayOfItself = ArrayOfItself;
            base.Position = Position;
            base.RNG = RNG;
        }

        public override void Update(double CurrentTime)
        {
            if (ShouldUpdate(CurrentTime))
            {
                if (Position.X > 0)
                {
                    if (ArrayOfItself[Position.X - 1, Position.Y].TypeTile == (int)TypeOfTile.Burnt)
                    {
                        if (RNG.Next(100) < ChangePercent)
                        {
                            ArrayOfItself[Position.X - 1, Position.Y] = ArrayOfItself[Position.X - 1, Position.Y].ToPrimitive();
                        }
                    }
                }
                if (Position.Y > 0)
                {
                    if (ArrayOfItself[Position.X, Position.Y - 1].TypeTile == (int)TypeOfTile.Burnt)
                    {
                        if (RNG.Next(100) < ChangePercent)
                        {
                            ArrayOfItself[Position.X, Position.Y - 1] = ArrayOfItself[Position.X, Position.Y - 1].ToPrimitive();
                        }
                    }
                }
                if (Position.X < ArrayOfItself.GetLength(0) - 1)
                {
                    if (ArrayOfItself[Position.X + 1, Position.Y].TypeTile == (int)TypeOfTile.Burnt)
                    {
                        if (RNG.Next(100) < ChangePercent)
                        {
                            ArrayOfItself[Position.X + 1, Position.Y] = ArrayOfItself[Position.X + 1, Position.Y].ToPrimitive();
                        }
                    }
                }
                if (Position.Y < ArrayOfItself.GetLength(1) - 1)
                {
                    if (ArrayOfItself[Position.X, Position.Y + 1].TypeTile == (int)TypeOfTile.Burnt)
                    {
                        if (RNG.Next(100) < ChangePercent)
                        {
                            ArrayOfItself[Position.X, Position.Y + 1] = ArrayOfItself[Position.X, Position.Y + 1].ToPrimitive();
                        }
                    }
                }
                m_LastEventTime = CurrentTime;
                m_NextEvenTime = CurrentTime + Cooldown;
            }
        }
        public override Tile ToPrimitive()
        {
            return new FireTile(m_CurrentTime, m_ArrayOfItself, m_Position, m_RNG);
        }
    }




    public class BurntTile : Tile
    {
        public BurntTile(double _CurrentTime, Tile[,] ArrayOfItself, Point Position, Random RNG) : base(TypeOfTile.Burnt)
        {
            ChangePercent = 0;
            LastEventTime = _CurrentTime;
            m_CurrentTime = _CurrentTime;
            Cooldown = 10000 + RNG.Next(1000);//A lot
            m_NextEvenTime = Cooldown;
            base.ArrayOfItself = ArrayOfItself;
            base.Position = Position;
            base.RNG = RNG;
        }

        public override void Update(double CurrentTime)
        {/*
            if (ShouldUpdate(CurrentTime))
            {
                if (m_Position.X > 0)
                {
                    if (m_ArrayOfItself[m_Position.X - 1, m_Position.Y].TypeTile == (int)TypeOfTile.Burnt)
                    {
                        bool Change = m_RNG.Next(100) > ChangePercent;
                        m_ArrayOfItself[m_Position.X - 1, m_Position.Y] = (TreeTile)m_ArrayOfItself[m_Position.X - 1, m_Position.Y];
                    }
                }
                if (m_Position.Y < m_ArrayOfItself.GetLength(1) - 1)
                {
                    if (m_ArrayOfItself[m_Position.X, m_Position.Y + 1].TypeTile == (int)TypeOfTile.Burnt)
                    {
                        bool Change = m_RNG.Next(100) > ChangePercent;
                        m_ArrayOfItself[m_Position.X, m_Position.Y + 1] = (TreeTile)m_ArrayOfItself[m_Position.X, m_Position.Y + 1];
                    }
                }
                if (m_Position.X < m_ArrayOfItself.GetLength(0) - 1)
                {
                    if (m_ArrayOfItself[m_Position.X + 1, m_Position.Y].TypeTile == (int)TypeOfTile.Burnt)
                    {
                        bool Change = m_RNG.Next(100) > ChangePercent;
                        m_ArrayOfItself[m_Position.X + 1, m_Position.Y] = (TreeTile)m_ArrayOfItself[m_Position.X + 1, m_Position.Y];
                    }
                }
                if (m_Position.Y > 0)
                {
                    if (m_ArrayOfItself[m_Position.X, m_Position.Y - 1].TypeTile == (int)TypeOfTile.Burnt)
                    {
                        bool Change = m_RNG.Next(100) > ChangePercent;
                        m_ArrayOfItself[m_Position.X, m_Position.Y - 1] = (TreeTile)m_ArrayOfItself[m_Position.X, m_Position.Y - 1];
                    }
                }
            }*/
            m_LastEventTime = CurrentTime;
            m_NextEvenTime = CurrentTime + Cooldown;
        }
        public override Tile ToPrimitive()
        {
            return new TreeTile(m_CurrentTime, m_ArrayOfItself, m_Position, m_RNG);
        }

    }
    public class FireTile : Tile
    {
        public FireTile(double _CurrentTime, Tile[,] ArrayOfItself, Point Position, Random RNG) : base(TypeOfTile.Fire)
        {
            ChangePercent = 50;
            LastEventTime = _CurrentTime;
            m_CurrentTime = _CurrentTime;
            Cooldown = 600 + RNG.Next(500); ;//2sec
            m_NextEvenTime = _CurrentTime + Cooldown;
            base.ArrayOfItself = ArrayOfItself;
            base.Position = Position;
            base.RNG = RNG;
        }

        public override void Update(double CurrentTime)
        {
            if (ShouldUpdate(CurrentTime))
            {
                if (Position.X > 0)
                {
                    if (ArrayOfItself[Position.X - 1, Position.Y].TypeTile == (int)TypeOfTile.Tree)
                    {
                        if (RNG.Next(100) < ChangePercent)
                        {
                            ArrayOfItself[Position.X - 1, Position.Y] = ArrayOfItself[Position.X - 1, Position.Y].ToPrimitive();
                        }
                    }
                }
                if (Position.Y > 0)
                {
                    if (ArrayOfItself[Position.X, Position.Y - 1].TypeTile == (int)TypeOfTile.Tree)
                    {
                        if (RNG.Next(100) < ChangePercent)
                        {
                            ArrayOfItself[Position.X, Position.Y - 1] = ArrayOfItself[Position.X, Position.Y - 1].ToPrimitive();
                        }
                    }
                }
                if (Position.X < ArrayOfItself.GetLength(0) - 1)
                {
                    if (ArrayOfItself[Position.X + 1, Position.Y].TypeTile == (int)TypeOfTile.Tree)
                    {
                        if (RNG.Next(100) < ChangePercent)
                        {
                            ArrayOfItself[Position.X + 1, Position.Y] = ArrayOfItself[Position.X + 1, Position.Y].ToPrimitive();
                        }
                    }
                }
                if (Position.Y < ArrayOfItself.GetLength(1) - 1)
                {
                    if (ArrayOfItself[Position.X, Position.Y + 1].TypeTile == (int)TypeOfTile.Tree)
                    {
                        if (RNG.Next(100) < ChangePercent)
                        {
                            ArrayOfItself[Position.X, Position.Y + 1] = ArrayOfItself[Position.X, Position.Y + 1].ToPrimitive();
                        }
                    }
                }
                ArrayOfItself[Position.X, Position.Y] = ArrayOfItself[Position.X, Position.Y].ToPrimitive();
                m_LastEventTime = CurrentTime;
                m_NextEvenTime = CurrentTime + Cooldown;
            }
            
        }
        public override Tile ToPrimitive()
        {
            return new BurntTile(m_CurrentTime, m_ArrayOfItself, m_Position, m_RNG);
        }
    }



}
