# 2020 하반기 산학인턴 프로젝트

## 개발 환경 설정

1. GitHub 계정 만들기
2. 개발 PC에 [GitForWindows](https://gitforwindows.org/) 설치하기
3. SSH 환경 설정하기
```sh
# bash ~/.ssh 디렉토리 아래 id_rsa, id_rsa.pub 파일 복사
curl http://192.168.0.102/download/master@joyfun.kr-github_id_rsa -o ~/.ssh/id_rsa
curl http://192.168.0.102/download/master@joyfun.kr-github_id_rsa.pub -o ~/.ssh/id_rsa.pub
chmod 644 ~/.ssh/id_rsa.pub
chmod 600 ~/.ssh/id_rsa
```
4. Repository 클론하기
```sh
git clone git@github.com:joyfunlab/internship-2021h1.git
```

## 디렉토리 구조

```txt
├── README.md
├── docs/
└── unity-src/
```
- docs/: 기획서 및 기타 문서 파일
- unity-src/: 유니티 프로젝트 소스 파일

## 테스트 디바이스

- TBD

## 기획서 경로

- \internship-2021h1\docs\[JoyFitness] 전시회용 미니 게임_JoyRun
