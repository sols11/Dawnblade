using System;
using UnityEngine;

namespace SFramework
{
    /// <summary>
    /// IPlayer,IPlayerMono,IPlayerWeapon3个关联类的中介者
    /// 负责更换装备
    /// </summary>
    public class PlayerMediator
    {
        public IPlayer Player { get; set; }
        public IPlayerMono PlayerMono { get; set; }
        public IPlayerWeapon PlayerWeapon { get; set; }

        public PlayerMediator(IPlayer _player)
        {
            Player = _player;
        }

        public void Initialize()
        {
            PlayerMono = Player.GameObjectInScene.GetComponent<IPlayerMono>();
            if (PlayerMono)
            {
                PlayerMono.PlayerMedi= this;
                PlayerMono.Rgbd = Player.Rgbd;
                PlayerMono.AnimatorComponent = Player.animator;
                PlayerWeapon = PlayerMono.iPlayerWeapon;
                PlayerMono.Initialize();
                if (PlayerWeapon != null)
                {
                    PlayerWeapon.PlayerMedi = this; // 引用
                    PlayerWeapon.Initialize();
                    PlayerMono.WeaponCollider = PlayerWeapon.WeaponCollider;
                }
                else
                    Debug.LogError("iPlayerWeapon未赋值");
                UpdatePlayerWeapon(PlayerMono.iPlayerWeapon);
            }
        }

        /// <summary>
        /// 用于从装备更新角色和武器属性
        /// </summary>
        /// <param name="equip"></param>
        public void FitEquip()
        {
            // 默认属性，以后要改的话可以存到存档里
            Player.MaxHP = 200;
            Player.MaxSP = 100;
            Player.MoveSpeed = 6;
            Player.RotSpeed = 12;
            Player.AttackPoint = 0;
            Player.DefendPoint = 0;
            Player.CritPoint = 0;
            Player.Special = SpecialAbility.无;
            // 装备属性
            for (int i = 0; i < Player.Fit.Length; i++)
            {
                if (Player.Fit[i] != null)
                    DressOnEquip(Player.Fit[i]);
            }
            UpdatePlayerWeapon(PlayerWeapon);
        }

        #region 备用，以后要替换装备时用
        private void DropEquip(IEquip _dropEquip)
        {
            if (_dropEquip != null)
            {
                //Player.MoveSpeed -= _dropEquip.Speed / 10;
                Player.MaxHP -= _dropEquip.HP;
                Player.MaxSP -= _dropEquip.SP;
                Player.AttackPoint -= _dropEquip.Attack;
                Player.DefendPoint -= _dropEquip.Defend;
                Player.CritPoint -= _dropEquip.Crit;
                if (_dropEquip.Type == FitType.Weapon)
                    Player.Special = SpecialAbility.无;
                //Player.Fit[(int)_dropEquip.Type] = null;
            }
        }

        private void DressOnEquip(IEquip _dressOnEquip)
        {
            //Player.MoveSpeed += _dressOnEquip.Speed / 10;
            Player.MaxHP += _dressOnEquip.HP;
            Player.MaxSP += _dressOnEquip.SP;
            Player.AttackPoint += _dressOnEquip.Attack;
            Player.DefendPoint += _dressOnEquip.Defend;
            Player.CritPoint += _dressOnEquip.Crit;
            if (_dressOnEquip.Type == FitType.Weapon)
                Player.Special = _dressOnEquip.Special;
        }
        #endregion

        /// <summary>
        /// 设置WeaponData，使用的是装备的武器
        /// 做强化系统时再加上强化的数值
        /// </summary>
        /// <param name="_weapon">哪一个PlayerWeapon</param>
        public void UpdatePlayerWeapon(IPlayerWeapon _playerWeapon)
        {
            _playerWeapon.BasicAttack = Player.AttackPoint;
            _playerWeapon.Crit = Player.CritPoint;
            _playerWeapon.Special = Player.Special;
        }

    }
}
