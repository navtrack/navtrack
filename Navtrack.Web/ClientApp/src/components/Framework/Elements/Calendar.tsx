import React, { useState } from "react";
import Icon from "components/framework/Util/Icon";
import moment, { Moment } from "moment";
import classNames from "classnames";

type Props = {
  date: Moment;
  setDate: (date: moment.Moment) => void;
  hide: () => void;
};

export default function DatePicker(props: Props) {
  const [date, setDate] = useState(props.date);
  const monthArray = getMonthArray(date);

  const setDay = (day: string) => {
    if (day) {
      props.setDate(moment(date).date(parseInt(day)));
      props.hide();
    }
  };
  const subtractMonth = () => setDate(moment(date).subtract(1, "month"));
  const addMonth = () => setDate(moment(date).add(1, "month"));

  return (
    <div
      className="shadow-md rounded p-3 bg-white"
      onClick={e => {
        e.stopPropagation();
        e.nativeEvent.stopImmediatePropagation();
      }}>
      <div className="flex flex-row mb-1 text-sm">
        <div
          className="px-1 w-8 text-center cursor-pointer hover:bg-gray-300"
          onClick={subtractMonth}>
          <Icon className="fa-chevron-left" />
        </div>
        <div className="flex-grow text-center">{moment(date).format("MMMM YYYY")}</div>
        <div className="px-1 w-8 text-center cursor-pointer hover:bg-gray-300" onClick={addMonth}>
          <Icon className="fa-chevron-right" />
        </div>
      </div>
      <table className="text-sm" style={{ width: "210px" }}>
        <thead>
          <tr>
            <td className="w-20 text-center px-1">Mon</td>
            <td className="w-20 text-center px-1">Tue</td>
            <td className="w-20 text-center px-1">Wed</td>
            <td className="w-20 text-center px-1">Thu</td>
            <td className="w-20 text-center px-1">Fri</td>
            <td className="w-20 text-center px-1">Sat</td>
            <td className="w-20 text-center px-1">Sun</td>
          </tr>
        </thead>
        <tbody>
          {monthArray.map((week, i) => (
            <tr key={i}>
              {week.map((day, j) => {
                return (
                  <td
                    key={j}
                    className={classNames("w-20 text-center ", {
                      "hover:bg-gray-300 cursor-pointer": day,
                      "bg-gray-300": date.date() === parseInt(day)
                    })}
                    onClick={() => setDay(day)}>
                    {day}
                  </td>
                );
              })}
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

const getMonthArray = (date: Moment) => {
  const firstDay = moment(date).startOf("month");
  const lastDay = moment(date).endOf("month");
  const daysInMonth = moment(lastDay).date();

  const firstDayinArray = moment(firstDay).isoWeekday();
  const noOfWeeks = Math.ceil((daysInMonth - (7 - firstDayinArray)) / 7) + 1;

  const days = getArray(7);
  const weeks = getArray(noOfWeeks);

  return weeks.map(week =>
    days.map(day => {
      let date = moment(firstDay).add(week * 7 + day - firstDayinArray + 1, "day");

      if (firstDay > date || date > lastDay) {
        return "";
      }

      return `${date.date()}`;
    })
  );
};

const getArray = (endIndex: number): number[] => {
  let a: number[] = [];

  for (let i = 0; i < endIndex; i++) {
    a.push(i);
  }

  return a;
};
