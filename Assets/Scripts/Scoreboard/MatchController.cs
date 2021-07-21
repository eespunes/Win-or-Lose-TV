using UnityEngine;
using UnityEngine.InputSystem;

namespace Scoreboard
{
    public class MatchController
    {
        private float _time;

        public bool StartHalf { get; private set; }
        private bool _timeout;

        private Team _homeTeam, _awayTeam;
        private ScoreboardGUI _scoreboardGui;
        private bool _introPressed;

        private readonly InputController inputController;
        private bool _playingIntro;


        public MatchController(ScoreboardGUI scoreboardGui, Team homeTeam, Team awayTeam)
        {
            this._scoreboardGui = scoreboardGui;
            this._homeTeam = homeTeam;
            this._awayTeam = awayTeam;
            StartHalf = true;
            scoreboardGui.HomeScore.text = this._homeTeam.PlayingScore.ToString();
            scoreboardGui.AwayScore.text = this._awayTeam.PlayingScore.ToString();

            inputController = new InputController();
            inputController.MatchController.Enable();
            inputController.MatchController.StartEndTime.performed += ctx => OnStartEndTime();
            inputController.MatchController.HomeGoalUp.performed += ctx => OnHomeGoalUp();
            inputController.MatchController.HomeGoalDown.performed += ctx => OnHomeGoalDown();
            inputController.MatchController.HomeFault.performed += ctx => OnHomeFault();
            inputController.MatchController.AwayGoalUp.performed += ctx => OnAwayGoalUp();
            inputController.MatchController.AwayGoalDown.performed += ctx => OnAwayGoalDown();
            inputController.MatchController.AwayFault.performed += ctx => OnAwayFault();
            inputController.MatchController.Timeout.performed += ctx => OnTimeout();
        }

        private bool PressingButtonCondition()
        {
            return Playing && _scoreboardGui.CanPressButton;
        }

        private void OnStartEndTime()
        {
            if (_introPressed)
                StartTime();
            else if (!_playingIntro)
            {
                _playingIntro = true;
                _scoreboardGui.StartCoroutine(_scoreboardGui.PreMatch());
            }
        }

        private void OnHomeGoalUp()
        {
            if (PressingButtonCondition())
            {
                IncreaseScoreHome();
            }
        }

        private void OnHomeGoalDown()
        {
            if (PressingButtonCondition())
                DecreaseScoreHome();
        }

        private void OnHomeFault()
        {
            if (_scoreboardGui.CanPressButton)
                HomeFault();
        }

        private void OnAwayGoalUp()
        {
            if (PressingButtonCondition())
                IncreaseScoreAway();
        }

        private void OnAwayGoalDown()
        {
            if (PressingButtonCondition())
                DecreaseScoreAway();
        }

        private void OnAwayFault()
        {
            if (_scoreboardGui.CanPressButton)
                AwayFault();
        }

        private void OnTimeout()
        {
            if (PressingButtonCondition())
                StartTimeout();
        }

        public void Update()
        {
            if (Playing)
            {
                UpdateTime();
            }
        }

        // TIME CONTROLLER
        private void StartTime()
        {
            if (Playing)
            {
                if (MatchConfig.GetInstance().StoppedTime || EndFirstHalf() || EndSecondHalf())
                    StopTime();
            }
            else if (!_timeout)
            {
                if (StartHalf)
                {
                    _time = MatchConfig.GetInstance().StoppedTime ? MatchConfig.GetInstance().MaxTime * 60 :
                        !FirstHalf ? 0 : MatchConfig.GetInstance().MaxTime * 60;
                    Playing = true;
                    StartHalf = false;
                    ChangeHalf();

                    if (FirstHalf)
                    {
                        _homeTeam.StartMatch();
                        _awayTeam.StartMatch();
                    }
                }

                Playing = true;

                _scoreboardGui.StartCoroutine(
                    _scoreboardGui.StopUpperOrStartBottom(MatchConfig.GetInstance().StoppedTime));
            }
        }

        private void UpdateTime()
        {
            if (MatchConfig.GetInstance().StoppedTime)
                _time -= 1 * Time.deltaTime;
            else
                _time += 1 * Time.deltaTime;

            int lMinutesInt = (int) _time / 60;
            string lAddMinutes = lMinutesInt < 10 ? "0" : "";

            float lSecondsFloat = _time % 60;
            string lAddSeconds = lSecondsFloat < 10 ? "0" : "";


            var commaSplit = lSecondsFloat.ToString("f2").Split(',');

            string finalString = lMinutesInt == 0
                ? lAddSeconds +
                  (commaSplit.Length != 2 ? lSecondsFloat.ToString("f2") : commaSplit[0] + "." + commaSplit[1])
                : lAddMinutes + lMinutesInt + ":" + lAddSeconds + ((int) lSecondsFloat).ToString("f0");

            if (lMinutesInt == 0 && lSecondsFloat <= 0)
            {
                _time = 0;
                finalString = "00.00";
                if (MatchConfig.GetInstance().StoppedTime)
                    StopTime();
            }

            _scoreboardGui.Time.text = finalString;

            if (MatchConfig.GetInstance().StoppedTime && _time == 0)
            {
            }
        }

        private void StopTime()
        {
            Playing = false;

            if (!MatchConfig.GetInstance().StoppedTime)
            {
                _scoreboardGui.StartCoroutine(
                    _scoreboardGui.StopUpperOrStartBottom(MatchConfig.GetInstance().StoppedTime));
                if (FirstHalf)
                {
                    _time = MatchConfig.GetInstance().MaxTime;
                    StartHalf = true;
                    _scoreboardGui.StartCoroutine(_scoreboardGui.HalfMatch());
                }
                else if (SecondHalf)
                {
                    _time = MatchConfig.GetInstance().MaxTime * 2;
                    StartHalf = true;
                    _scoreboardGui.StartCoroutine(_scoreboardGui.EndMatch());
                }

                _scoreboardGui.Time.text = _time + ":00";
            }
            else if (_time == 0)
            {
                StartHalf = true;
                _scoreboardGui.StartCoroutine(
                    _scoreboardGui.StopUpperOrStartBottom(MatchConfig.GetInstance().StoppedTime));
                if (FirstHalf)
                {
                    _scoreboardGui.StartCoroutine(_scoreboardGui.HalfMatch());
                }
                else if (SecondHalf)
                {
                    _scoreboardGui.StartCoroutine(_scoreboardGui.EndMatch());
                }
            }
        }

        public bool FirstHalf { get; private set; }

        public bool SecondHalf { get; private set; }

        // HALF CONTROLLER
        private void ChangeHalf()
        {
            if (!FirstHalf && !SecondHalf)
            {
                FirstHalf = true;
                // _scoreboardGui.Half.text = "1";
            }
            else if (FirstHalf)
            {
                FirstHalf = false;
                SecondHalf = true;
                // _scoreboardGui.Half.text = "2";
            }

            ResetFaults();
        }

        private void EndHalf()
        {
            Playing = false;
            StartHalf = true;
            if (FirstHalf)
                _scoreboardGui.StartCoroutine(_scoreboardGui.HalfMatch());

            else if (SecondHalf)
                _scoreboardGui.StartCoroutine(_scoreboardGui.EndMatch());


            _scoreboardGui.StartCoroutine(_scoreboardGui.StopUpperOrStartBottom(false));
        }

        private bool EndFirstHalf()
        {
            return Playing && _time / 60 >= MatchConfig.GetInstance().MaxTime - 1 && FirstHalf;
        }

        private bool EndSecondHalf()
        {
            return Playing && _time / 60 >= MatchConfig.GetInstance().MaxTime * 2 - 1 && SecondHalf;
        }

        // TIMEOUT CONTROLLER
        private void StartTimeout()
        {
            _timeout = true;
            Playing = false;
            _scoreboardGui.StartCoroutine(_scoreboardGui.Timeout());
        }

        public void StopTimeout()
        {
            _timeout = false;
            if (!MatchConfig.GetInstance().StoppedTime)
                StartTime();
        }

        // FAULTS CONTROLLER
        private void HomeFault()
        {
            _homeTeam.IncreaseFault();
            _scoreboardGui.StartCoroutine(_scoreboardGui.HomeFaults(_homeTeam.PlayingFaults));
        }

        private void AwayFault()
        {
            _awayTeam.IncreaseFault();
            _scoreboardGui.StartCoroutine(_scoreboardGui.AwayFaults(_awayTeam.PlayingFaults));
        }

        private void ResetFaults()
        {
            _homeTeam.ResetFaults();
            _scoreboardGui.ResetFaults();
            _awayTeam.ResetFaults();
        }

        //GOALS CONTROLLER
        private void IncreaseScoreHome()
        {
            _scoreboardGui.StopAllCoroutines();
            if (MatchConfig.GetInstance().StoppedTime)
                StopTime();

            _scoreboardGui.StartCoroutine(_scoreboardGui.Goal("Home Goal"));
        }

        public void IncreaseScoreHomeUI()
        {
            _homeTeam.IncreaseScore(_awayTeam);
            _scoreboardGui.HomeScore.text = _homeTeam.PlayingScore.ToString();
        }

        private void IncreaseScoreAway()
        {
            _scoreboardGui.StopAllCoroutines();
            if (MatchConfig.GetInstance().StoppedTime)
                StopTime();

            _scoreboardGui.StartCoroutine(_scoreboardGui.Goal("Away Goal"));
        }

        public void IncreaseScoreAwayUI()
        {
            _awayTeam.IncreaseScore(_homeTeam);
            _scoreboardGui.AwayScore.text = _awayTeam.PlayingScore.ToString();
        }

        private void DecreaseScoreHome()
        {
            _homeTeam.DecreaseScore(_awayTeam);
            _scoreboardGui.HomeScore.text = _homeTeam.PlayingScore.ToString();
        }

        private void DecreaseScoreAway()
        {
            _awayTeam.DecreaseScore(_homeTeam);
            _scoreboardGui.AwayScore.text = _awayTeam.PlayingScore.ToString();
        }

        // GETTERS
        public bool Playing { get; set; }

        public bool Timeout => _timeout;

        public bool IntroPressed
        {
            get => _introPressed;
            set => _introPressed = value;
        }
    }
}