# 《代码管理：git 使用规范》

## 1. 工具下载

下载[Github desktop](https://desktop.github.com/)  
下载[Sourcetree](https://www.sourcetreeapp.com/)  
PS：一般来说上述两个工具选择一个使用即可。
    即用Sourcetree + Git Brash（命令行） 或者 Github desktop。
<br>

## 2. 项目创建
### 2.1 配置公钥
配置公钥的目的，是使得今后在进行git操作时，不用**每次都**需要进行身份验证。  
配置教程可参考[配置SSH公钥](https://coding.net/help/doc/git/ssh-key.html)  

### 2.2 克隆项目
    git clone https://github.com/MegicLand/MagicLand.git
<br>

## 3. 项目开发
### 3.1 分支管理
在此我们先给出分支管理的*逻辑*，在本节末给出完整的git指令
#### 3.1.1 **分支创建**
每开发一个功能：

  - 都应该基于当前最新的dev分支, 去创建一个新的分支
  - 该分支名应当*自解释*了当前分支负责开发的功能——给分支取一个有意义的名字**很重要**

假如你正在dev分支下，你应该使用

    git checkout -b your_new_branch

去创建一个，基于dev分支的，名为your_new_branch的新分支。

#### 3.1.2 将发生修改的文件添加到commit list

在完成当前分支开发后，需要做两件事：

  - 进行代码的自我审查（在3.2.1中描述）
  - 将发生修改的文件添加到commit list中，有两种方法可以实现这一点：
    - 通过*git add your_filename*指令，将与your_filename参数匹配的文件添加到commit list中
    - 通过*sourcetree*软件，在unstaged files标签中，选择文件将它添加到staged files中

#### 3.1.3 提交代码(commit)

通过git commit指令提交代码，注意：

  - -m参数后需要添加一段提交注释
  - 我们项目要求：注释应解释当前提交行为，使用英文，并以大写字母开头。  

示例：  

    git commit -m "Modify CMakeList.txt to add a console when program running."

#### 3.1.4 推送代码(rebase & push)

在推送代码之前，建议将分支 rebase 在最新的 dev 分支上：因为在 checkout 和 push 之间，有可能别的开发者更新了 dev 分支：

    git rebase dev

在成功rebase后，再将当前分支推送到远程仓库中：

    git push -u origin your_new_branch

#### 3.1.5 命令合集

开发过程应***至少***使用如下指令：

    git checkout -b your_new_branch
    ... // Development begins
    ...
    ... // Development ends
    git add ...
    git commit -m "Your meaningful commit message."
    git push -u origin your_new_branch

完成推送代码后，方可在 github 上发起合并请求。

### 3.2 代码审查

#### 3.2.1 自我审查

在两个环节应进行代码的自我审查：

  - 在3.1.2提交代码，进行git add指令时
  - 在3.1.4推送代码，进行git rebase指令时

审查的内容包括但不仅限于：

  - 代码是否符合C++标准规范(C++11, C++14)
  - 代码是否符合项目代码规范
    - suri基本采用Google C++ Code Style
    - huacaya应尽快确立自己的代码规范（可适当参考suri）
  - 代码是否可以写得更高效、更可读（简洁）

#### 3.2.2 他人审查

他人审查发生在分支被push到远程仓库**后**。  
项目成员角色暂划分为：

  - Reviewer（boss分支）: 赵志伟
  - Reviewer（player分支）: 梅哲凡

**3.2.2.1 Non-reviewer push**  
Non-reviewer(A) 在将分支 push 到远程仓库后，应在 github 上向某一位 reviewer(B) 发起向 dev 分支的合并请求，B 将负责代码的审查及分支合并。

**3.2.2.2 Reviwer push**  
Reviewer(A) 在将分支 push 到远程仓库后，应在 github 上向某一位 reviewer(B) 发起向 dev 分支的合并请求，B 将负责代码的审查及分支合并。

<br>

## 4. 练习

## 4.1 练习 1: 分支管理

1. 基于 dev 创建新的分支 A

    git checkout -b A

2. 在 A 下修改代码，并将代码推送到远程

    git commit -m "Your message."
    git push -u origin A

3. 删除远程的分支 A

    git push -u origin :A

4. checkout 回 dev, 删除本地的分支 A

    git branch -D A

## 4.2 练习 2: 通过 `git rebase -i` 将多个 commit points 压缩成一个

a) 基于 dev 创建新的分支 A  
b) 基于 dev 进行 3 次的 commit, commit 消息分别为

  - Temp commit1
  - Temp commit2
  - Temp commit3

c) 使用 git rebase 压缩 commit points

    git rebase -i HEAD~3

通过上述命令，请仔细阅读你将在 `vi` 或 `vim` 中看到的如下文本：


    pick bd99d06 Temp commit1
    pick 09a34e5 Temp commit2
    pick afac113 Temp commit3

    # Rebase d2d5718..afac113 onto d2d5718 (3 command(s))
    #
    # Commands:
    # p, pick = use commit
    # r, reword = use commit, but edit the commit message
    # e, edit = use commit, but stop for amending
    # s, squash = use commit, but meld into previous commit
    # f, fixup = like "squash", but discard this commit's log message
    # x, exec = run command (the rest of the line) using shell
    # d, drop = remove commit
    #
    # These lines can be re-ordered; they are executed from top to bottom.
    #
    # If you remove a line here THAT COMMIT WILL BE LOST.
    #
    # However, if you remove everything, the rebase will be aborted.

前三行的顺序是可以任意替换的，这将导致最终呈现的 commit 顺序不同。  
在此，我们要做的是压缩 commit2 和 commit3, 所以我们只需要将对应的 `pick` 改成 `squash` 或 `s`, 再保存退出 (`:wq`) 即可。

## 4.3 练习 3: 生成一份关于最新的 commit 的 patch

a) 基于 dev 创建新的分支 A  
b) commit 一次修改

    `git commit -m "My commit"`

c) **自己动手**，生成一份关于 `My commit` 的一份 patch

## 5. 总结
本文先给出了代码***管理工具***的下载地址，描述了项目的***创建方式***，再以***分支管理***、***代码审查***（自我审查与他人审查）为例，指出了在开发过程中应遵循的行为规范和实现方式。最后，提供了相关练习。  

希望大家在今后的开发过程中，能遵守规范，掌握在*命令行*或图形界面下进行代码管理的方法。
