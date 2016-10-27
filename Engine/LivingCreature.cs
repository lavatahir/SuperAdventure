using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Engine
{
    public class LivingCreature : INotifyPropertyChanged
    {
        private int _currentHitPoints;
        public int currentHitPoints
        {
            get { return _currentHitPoints; }
            set
            {
                _currentHitPoints = value;
                OnPropertyChanged("currentHitPoints");
            }
        }
        public int maxHitPoints;
        public event PropertyChangedEventHandler PropertyChanged;

        private int _strength;
        public int strength
        {
            get { return _strength; }
            set { _strength = value; }
        }

        private int _iq;
        public int iq
        {
            get { return _iq; }
            set { _iq = value; }
        }
        private int _quickness;
        public int quickness
        {
            get { return _quickness; }
            set { _quickness = value; }
        }
        public LivingCreature(int currentHitPoints, int maxHitPoints)
        {
            this.currentHitPoints = currentHitPoints;
            this.maxHitPoints = maxHitPoints;
            this.iq = DEFAULT;
            this.quickness = DEFAULT;
            this.strength = DEFAULT;
        }
        private const int DEFAULT = 5;
        public event EventHandler<MessageEventArgs> onMessage;




        protected void OnPropertyChanged(string name)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        protected bool canHit()
        {
            if(RandomNumberGenerator.numberBetween(0,1) == 1)
            {
                return true;
            }
            return false;
        }
        protected void RaiseMessage(string message, bool addExtraNewLine = false)
        {
            if (onMessage != null)
            {
                onMessage(this, new MessageEventArgs(message, addExtraNewLine));
            }
        }
    }
}