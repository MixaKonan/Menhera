CREATE TABLE `admin` (
    `admin_id` int(11) NOT NULL AUTO_INCREMENT,
    `login` varchar(45) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
    `email` varchar(45) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
    `password_hash` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
    `can_delete_posts` bit NOT NULL DEFAULT b'1',
    `can_close_threads` bit NOT NULL DEFAULT b'0',
    `has_access_to_panel` bit NOT NULL DEFAULT b'0',
    `can_ban_users` bit NOT NULL DEFAULT b'0',
    `admin_ip_hash` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
    CONSTRAINT `PK_admin` PRIMARY KEY (`admin_id`)
);

CREATE TABLE `board` (
    `board_id` int(11) NOT NULL AUTO_INCREMENT,
    `prefix` varchar(15) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
    `postfix` varchar(15) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
    `title` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
    `description` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
    `is_hidden` bit NOT NULL DEFAULT b'0',
    `anon_has_no_name` bit NOT NULL DEFAULT b'0',
    `has_subject` bit NOT NULL DEFAULT b'1',
    `files_are_allowed` bit NOT NULL DEFAULT b'1',
    `file_limit` smallint(6) NOT NULL DEFAULT '4',
    `anon_name` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT '',
    CONSTRAINT `PK_board` PRIMARY KEY (`board_id`)
);

CREATE TABLE `ban` (
    `ban_id` int(11) NOT NULL AUTO_INCREMENT,
    `admin_id` int(11) NOT NULL,
    `anon_ip_hash` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
    `ban_time_in_unix_seconds` bigint NOT NULL,
    `term` bigint(20) NOT NULL,
    `reason` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
    CONSTRAINT `PK_ban` PRIMARY KEY (`ban_id`),
    CONSTRAINT `FK_AdminBan` FOREIGN KEY (`admin_id`) REFERENCES `admin` (`admin_id`) ON DELETE RESTRICT
);

CREATE TABLE `thread` (
    `thread_id` int(11) NOT NULL AUTO_INCREMENT,
    `board_id` int(11) NOT NULL,
    `is_closed` bit NOT NULL DEFAULT b'0',
    `op_ip_hash` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
    `anon_name` varchar(45) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
    CONSTRAINT `PK_thread` PRIMARY KEY (`thread_id`),
    CONSTRAINT `FK_BoardThread` FOREIGN KEY (`board_id`) REFERENCES `board` (`board_id`) ON DELETE RESTRICT
);

CREATE TABLE `post` (
    `post_id` int(11) NOT NULL AUTO_INCREMENT,
    `board_id` int(11) NOT NULL,
    `thread_id` int(11) NOT NULL,
    `email` varchar(45) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
    `subject` varchar(45) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
    `comment` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
    `anon_name` varchar(45) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
    `is_pinned` bit NOT NULL DEFAULT b'0',
    `time_in_unix_seconds` bigint NOT NULL,
    `anon_ip_hash` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
    CONSTRAINT `PK_post` PRIMARY KEY (`post_id`),
    CONSTRAINT `FK_BoardPost` FOREIGN KEY (`board_id`) REFERENCES `board` (`board_id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_ThreadPost` FOREIGN KEY (`thread_id`) REFERENCES `thread` (`thread_id`) ON DELETE CASCADE
);

CREATE TABLE `report` (
    `report_id` int(11) NOT NULL AUTO_INCREMENT,
    `board_id` int(11) NOT NULL,
    `thread_id` int(11) NOT NULL,
    `post_id` int(11) NOT NULL,
    `reason` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
    `report_time_in_unix_seconds` bigint NOT NULL,
    CONSTRAINT `PK_report` PRIMARY KEY (`report_id`),
    CONSTRAINT `FK_BoardReport` FOREIGN KEY (`board_id`) REFERENCES `board` (`board_id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_ThreadReport` FOREIGN KEY (`thread_id`) REFERENCES `thread` (`thread_id`) ON DELETE RESTRICT
);

CREATE TABLE `file` (
    `file_id` int(11) NOT NULL AUTO_INCREMENT,
    `board_id` int(11) NOT NULL,
    `thread_id` int(11) NOT NULL,
    `post_id` int(11) NOT NULL,
    `file_name` varchar(45) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
    `thumbnail_name` varchar(45) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
    `info` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
    CONSTRAINT `PK_file` PRIMARY KEY (`file_id`),
    CONSTRAINT `FK_BoardFile` FOREIGN KEY (`board_id`) REFERENCES `board` (`board_id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_PostFile` FOREIGN KEY (`post_id`) REFERENCES `post` (`post_id`) ON DELETE CASCADE,
    CONSTRAINT `FK_ThreadFile` FOREIGN KEY (`thread_id`) REFERENCES `thread` (`thread_id`) ON DELETE CASCADE
);

CREATE INDEX `fkAdminId_Ban` ON `ban` (`admin_id`);

CREATE UNIQUE INDEX `prefix` ON `board` (`prefix`);

CREATE INDEX `fkBoardId_File` ON `file` (`board_id`);

CREATE INDEX `fkPostId_File` ON `file` (`post_id`);

CREATE INDEX `fkThreadId_File` ON `file` (`thread_id`);

CREATE INDEX `fkBoardId_Post` ON `post` (`board_id`);

CREATE INDEX `fkThreadId_Post` ON `post` (`thread_id`);

CREATE INDEX `fkBoardId_Report` ON `report` (`board_id`);

CREATE INDEX `fkThreadId_Report` ON `report` (`thread_id`);

CREATE INDEX `fkBoardId_Thread` ON `thread` (`board_id`);

