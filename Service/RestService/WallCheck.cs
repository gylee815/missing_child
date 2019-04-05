using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestService
{
    public class WallCheck
    {
        //  강의실에서는 안뚫리는것 같음... , 첫번째 복도쪽에서는 양쪽 끝벽은 안뚫리는데 강의실쪽벽이 뚫림 해결해야됨...
        public bool check_wall(int x, int y)
        {
            if (x < 540 ||  x > 24500 || y < 200 || y > 29800)
                return true;
            else if(x >= 540 && x <= 7560)  //301호, 302호
            {
                if (y <= 8000)
                    return true;
                else if (y >= 29510)
                    return true;
                else if (y >= 19580 && y <= 18820)
                    return true;
                else
                    return false;
            }

            else if (x > 8220 && x < 9660) //첫번째 복도 (완료)
            {
                if (y <= 4600)
                    return true;
                else if (y > 29800)
                    return true;
                else
                    return false;
            }

            else if (x > 9940 && x < 13640) //중앙 복도 (완료)
            {
                if (y > 22500)
                    return true;
                else if (y < 19400)
                    return true;
                else
                    return false;
            }

            else if( x > 14120 && x < 15560)  //두번째 복도 (완료)
            {
                if (y > 21800)
                    return true;
                else
                    return false;
            }

            else
                return true;
        }
    }
}