//Названия, написанные с CapsLock соответствуют названиям таблиц в БД
public bool Add(int vReaderId, string vReaderDescr, TAG_REG_PARAMETER param)
{
    try
    {
        TAG_REG_NOTIFICATION notification;
        if (notifications.ContainsKey(vReaderDescr))
        {
            notifications.TryGetValue(vReaderDescr, out notification);
        }
        else
        {
            notification = new TAG_REG_NOTIFICATION("INVENTORY", vReaderId, vReaderDescr, EPCStates);
            notifications.Add(vReaderDescr, notification);
        }
        return notification.Add(param);
    }
    catch (Exception ex)
    {
        Console.WriteLine("TAG_REG_NOTIFICATIONS Add exception: " + ex.Message);
    }
    return false;
}

public bool AddInfo(TAG_REG_FULL_CURRENT_STATE param)
{
    try
    {
        int vReaderId = param.V_READER_ID;
        string vReaderDescr = param.V_READER_DESCR;
        TAG_REG_NOTIFICATION notification;
        if (notifications.ContainsKey(vReaderDescr))
        {
            notifications.TryGetValue(vReaderDescr, out notification);
        }
        else
        {
            notification = new TAG_REG_NOTIFICATION("INVENTORY", vReaderId, vReaderDescr, EPCStates);
            notifications.Add(vReaderDescr, notification);
        }

        notification.AddInfo(param);
        return true;
    }
    catch (Exception ex)
    {
        Console.WriteLine("TAG_REG_NOTIFICATIONS AddInfo exception: " + ex.Message);
    }
    return false;
}

public bool AddInfoToEmailSendingStates(TAG_REG_FULL_CURRENT_STATE param)
{
    try
    {
        string vReaderDescr = param.V_READER_DESCR;
        TAG_REG_NOTIFICATION notification;
        if (notifications.ContainsKey(vReaderDescr))
        {
            notifications.TryGetValue(vReaderDescr, out notification);
            notification.AddInfoToEmailSendingStates(param);
        }

        return true;
    }
    catch (Exception ex)
    {
        Console.WriteLine("TAG_REG_NOTIFICATIONS AddInfoToEmailSendingStates exception: " + ex.Message);
    }
    return false;
}

public List<Tuple<string, List<TAG_REG_PARAMETER>>> ToStrings(bool isUpdateStatusesInDB)
{
    List<Tuple<string, List<TAG_REG_PARAMETER>>> result = new List<Tuple<string, List<TAG_REG_PARAMETER>>>();
    foreach (var currentReaderNotifications in notifications)
    {
        List<Tuple<string, List<TAG_REG_PARAMETER>>> readerResult = currentReaderNotifications.Value.ToStrings(isUpdateStatusesInDB);
        for (int i = 0; i < readerResult.Count; ++i)
            result.Add(readerResult[i]);
    }
    return result;
}

public List<Tuple<string, List<TAG_REG_PARAMETER>>> ToEmailStrings(bool isForSaving)
{
    List<Tuple<string, List<TAG_REG_PARAMETER>>> result = new List<Tuple<string, List<TAG_REG_PARAMETER>>>();
    foreach (var currentReaderNotifications in notifications)
    {
        List<Tuple<string, List<TAG_REG_PARAMETER>>> readerResult = currentReaderNotifications.Value.ToEmailStrings(isForSaving);
        for (int i = 0; i < readerResult.Count; ++i)
            result.Add(readerResult[i]);
    }
    return result;
}