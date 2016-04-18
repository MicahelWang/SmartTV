ALTER TABLE `YeahTVTest`.`BackupDevice` 
ADD COLUMN `IsUsed` BIT(1) NULL DEFAULT b'0' AFTER `DeviceType`;